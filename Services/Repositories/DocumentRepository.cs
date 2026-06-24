using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;
using Services.Utils;
using Services.ViewModels._DocumentViewModels;

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


        public async Task<bool> DeleteAsync(Guid documentId, Guid submitUserId)
        {
            var document = await GetByIdAsync(documentId);
            if (document == null)
            {
                return false;
            }

            document.IsDeleted = true;
            document.DeletedDate = DateTime.Now;
            document.DeleteBy = submitUserId;

            return await UpdateAsync(document);
        }

        public async Task<PagedResult<DocumentItem>> GetAllAsync(GetDocumentListCriteria criteria)
        {
            var documents = from doc in _dbContext.Documents
                            join branch in _dbContext.Branchs
                                on doc.BranchId equals branch.Id

                            where !doc.IsDeleted
                            select new { doc, branch };

            if (!string.IsNullOrEmpty(criteria.SearchName))
            {
                documents = documents.Where(x => x.doc.Title.Contains(criteria.SearchName));
            }


            if (criteria.DocumentTypeId.HasValue)
            {
                var documentType = criteria.DocumentTypeId.Value.ToEnum<DocumentType>();
                documents = documents.Where(x => x.doc.DocumentType == documentType);
            }

            if (criteria.BranchId.HasValue)
            {
                documents = documents.Where(x => x.doc.BranchId == criteria.BranchId.Value);
            }

            var totalCount = await documents.
                                Select(x => x.doc.Id)
                                .Distinct()
                                .CountAsync();

            var items = await documents
                        .Select(x => new DocumentItem
                        {
                            DocumentId = x.doc.Id,
                            BranchId = x.branch.Id,
                            BranchName = x.branch.Name,
                            DocumentTitle = x.doc.Title,
                            DocumentType = x.doc.DocumentType,
                            DocumentStatus = x.doc.DocumentStatus,
                            DocumentDescription = x.doc.Description,
                            CoverImageUrl = x.doc.CoverImageUrl,
                            PublishDate = x.doc.PublishDate,
                        })
                        .Distinct()
                        .Skip((criteria.Page - 1) * criteria.Page)
                        .Take(criteria.RowsPerPage)
                        .ToListAsync();

            var pagedResult = new PagedResult<DocumentItem>(items, totalCount, criteria.Page, criteria.RowsPerPage);
            return pagedResult;
        }

        #region Client
        public async Task<PagedResult<DocumentItem>> GetListDocumentItem(GetDocumentItemCriteria criteria)
        {
            var documentsQuery = from doc in _dbContext.Documents
                                 join branch in _dbContext.Branchs
                                     on doc.BranchId equals branch.Id

                                 join br in _dbContext.BorrowRequest
                                     on doc.Id equals br.DocumentId into borrowGroup

                                 from borrow in borrowGroup.DefaultIfEmpty()
                                 where 
                                 !doc.IsDeleted
                                 && !branch.IsDeleted
                                 orderby doc.DocumentStatus ascending
                                 select new { doc, branch, borrow };

            if (criteria.BranchId.HasValue)
            {
                documentsQuery = documentsQuery.Where(x => x.doc.BranchId == criteria.BranchId.Value);
            }

            if (!string.IsNullOrEmpty(criteria.SearchDocumentName))
            {
                documentsQuery = documentsQuery
                    .Where(x => x.doc.Title.Contains(criteria.SearchDocumentName));
            }

            if (criteria.DocumentTypes.Any())
            {
                documentsQuery = documentsQuery.Where(x => criteria.DocumentTypes.Contains(x.doc.DocumentType));
            }

            if (criteria.StartDate.HasValue)
            {
                documentsQuery = documentsQuery.Where(x => x.doc.PublishDate >= criteria.StartDate);
            }

            if (criteria.EndDate.HasValue)
            {
                documentsQuery = documentsQuery.Where(x => x.doc.PublishDate <= criteria.EndDate);
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
                            BranchName = x.branch.Name,
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

        #endregion
    }
}
