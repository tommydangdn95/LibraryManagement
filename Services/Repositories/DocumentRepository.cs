using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
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

        public async Task<PagedResult<DocumentItem>> GetListDocumentItem(GetDocumentItemCriteria criteria)
        {
            var documentsQuery = from doc in _dbContext.Documents
                                 join db in _dbContext.DocumentBranchs
                                     on doc.Id equals db.DocumentId
                                 join branch in _dbContext.Branchs
                                     on db.BranchId equals branch.Id
                                 join br in _dbContext.BorrowRecords
                                     on doc.Id equals br.DocumentId into borrowGroup

                                 from borrow in borrowGroup.DefaultIfEmpty()
                                 where !doc.IsDeleted
                                 select new { doc, branch, borrow };

            if (criteria.BranchId.HasValue)
            {
                var documentIdsInBranch = _dbContext.DocumentBranchs.Where(db => db.BranchId == criteria.BranchId.Value)
                                                    .Select(db => db.DocumentId);
                documentsQuery = documentsQuery.Where(x => documentIdsInBranch.Contains(x.doc.Id));
            }

            if (!string.IsNullOrEmpty(criteria.SearchDocumentName))
            {
                documentsQuery = documentsQuery
                    .Where(x => x.doc.Title.Contains(criteria.SearchDocumentName));
            }

            var totalCount = await documentsQuery
                                    .Select(x => x.doc.Id)
                                    .Distinct()
                                    .CountAsync();

            var items = await documentsQuery
                        .Select(x => new DocumentItem
                        {
                            DocumentId = x.doc.Id,
                            BranchId = x.branch.Id,
                            DocumentTitle = x.doc.Title,
                            DocumentType = x.doc.DocumentType,
                            DocumentStatus = x.doc.DocumentStatus,
                            DocumentDescription = x.doc.Description,
                            CoverImageUrl = x.doc.CoverImageUrl,
                            PublishDate = x.doc.PublishDate,
                            BorrowStatus = x.borrow != null ? x.borrow.BorrowStatus : (BorrowStatus?)null,
                            BorrowDate = x.borrow != null ? x.borrow.BorrowDate : null,
                            ReturnDate = x.borrow != null ? x.borrow.ReturnDate : null,
                        })
                        .Distinct()
                        .Skip((criteria.Page - 1) * criteria.Page)
                        .Take(criteria.RowsPerPage)
                        .ToListAsync();

            var pagedResult = new PagedResult<DocumentItem>(items, totalCount, criteria.Page, criteria.RowsPerPage);
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
