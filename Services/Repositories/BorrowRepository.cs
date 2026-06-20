using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;

namespace Services.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly AppDbContext _dbContext;
        public BorrowRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(BorrowRequest borrowRequest)
        {
            await _dbContext.BorrowRequest.AddAsync(borrowRequest);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid borrowRequestId, Guid submitUserId)
        {
            var borrowRequest = await GetById(borrowRequestId);
            if (borrowRequest == null) 
            {
                return false;
            }

            borrowRequest.DeletedDate = DateTime.Now;
            borrowRequest.DeleteBy = submitUserId;
            borrowRequest.IsDeleted = true;
            return await UpdateAsync(borrowRequest);
        }

        public async Task<PagedResult<DocumentItem>> GetBorrowDocumentByUser(GetDocumentListBorrowItemCriteria criteria, Guid userId)
        {
            var query = from doc in _dbContext.Documents
                        join docB in _dbContext.DocumentBranchs
                            on doc.Id equals docB.DocumentId
                        join branch in _dbContext.Branchs
                            on docB.BranchId equals branch.Id

                        join borrow in _dbContext.BorrowRequest
                            on doc.Id equals borrow.DocumentId 

                        where !doc.IsDeleted 
                             && borrow.BorrowerId == userId
                             && (!criteria.BorrowStatuses.Any() || criteria.BorrowStatuses.Contains(borrow.BorrowStatus))
                        select new { doc, branch, borrow };

            var totalCount = await query
                                    .Select(x => x.doc.Id)
                                    .Distinct()
                                    .CountAsync();

            var items = await query
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

        public Task<BorrowRequest> GetById(Guid borrowRequestId)
        {
            return _dbContext.BorrowRequest.FirstOrDefaultAsync(x => x.Id == borrowRequestId);
        }

        public async Task<bool> UpdateAsync(BorrowRequest borrowRequest)
        {
            _dbContext.BorrowRequest.Update(borrowRequest);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
