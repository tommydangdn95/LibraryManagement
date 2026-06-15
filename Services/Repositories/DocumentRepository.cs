using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;
using Services.Utils;

namespace Services.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _dbContext;
        public DocumentRepository(AppDbContext appDbContext)
        {
            this._dbContext = appDbContext;
        }

        public async Task<bool> CreateAsync(Document document)
        {
            await _dbContext.Documents.AddAsync(document);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Document document)
        {
            _dbContext.Documents.Update(document);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Document> GetByIdAsync(Guid documentId)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted);
            return document;
        }

        public async Task<PagedResult<Document>> GetAllAsync(GetDocumentListCriteria criteria)
        {
            var documents = _dbContext.Documents.Where(x => !x.IsDeleted);



            if (!string.IsNullOrEmpty(criteria.SearchName))
            {
                documents = documents.Where(x => x.Title.Contains(criteria.SearchName));
            }


            if (criteria.DocumentTypeId.HasValue)
            {
                var documentType = criteria.DocumentTypeId.Value.ToEnum<DocumentType>();
                documents = documents.Where(x => x.DocumentType == documentType);
            }

            if (criteria.BranchId.HasValue)
            {
                documents = documents.Include(d => d.DocumentBranches)
                                     .ThenInclude(b => b.Branch)
                                     .Where(x => x.DocumentBranches.Any(b => b.Branch.Id == criteria.BranchId.Value));
            }

            var count = await documents.CountAsync();
            var items = await documents.Skip((criteria.Page - 1) * criteria.RowsPerPage)
                                 .Take(criteria.RowsPerPage).ToListAsync();

            var pagedResult = new PagedResult<Document>(items, count, criteria.Page, criteria.RowsPerPage);
            return pagedResult;
        }

        public async Task<bool> CreateDocumentBranch(DocumentBranch documentBranch)
        {
            await _dbContext.DocumentBranchs.AddAsync(documentBranch);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
