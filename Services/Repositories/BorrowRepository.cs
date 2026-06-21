using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Borrow;
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


        public async Task<PagedResult<BorrowRequestItem>> GetBorrowRequestItem(GetListBorrowRequestItemCriteria criteria)
        {
            var query = from borrow in _dbContext.BorrowRequest
                        join doc in _dbContext.Documents
                            on borrow.DocumentId equals doc.Id

                        join docB in _dbContext.DocumentBranchs
                            on doc.Id equals docB.DocumentId

                        join branch in _dbContext.Branchs
                            on docB.BranchId equals branch.Id

                        join user in _dbContext.Users
                            on borrow.BorrowerId equals user.Id

                        where !doc.IsDeleted
                             && (!criteria.BorrowStatuses.Any() || criteria.BorrowStatuses.Contains(borrow.BorrowStatus))
                        select new { borrow, doc, branch, user  };

            var totalCount = await query
                                    .Select(x => x.branch.Id)
                                    .Distinct()
                                    .CountAsync();

            var items = await query
                        .Select(x => new BorrowRequestItem
                        {
                            BrrowRequestId = x.borrow.Id,
                            DocumentId = x.doc.Id,
                            BranchId = x.branch.Id,
                            BranchName = x.branch.Name,
                            BorrowerId = x.borrow.BorrowerId,
                            BorrowerName = x.user.FullName,
                            DocumentTitle = x.doc.Title,
                            DocumentType = x.doc.DocumentType,
                            DocumentStatus = x.doc.DocumentStatus,
                            DocumentDescription = x.doc.Description,
                            CoverImageUrl = x.doc.CoverImageUrl,
                            PublishDate = x.doc.PublishDate,
                            BorrowStatus = x.borrow.BorrowStatus,
                            RequestDate = x.borrow.BorrowDate.Value,
                            ReturnDate = x.borrow.ReturnDate.Value,
                        })
                        .Distinct()
                        .Skip((criteria.Page - 1) * criteria.Page)
                        .Take(criteria.RowsPerPage)
                        .ToListAsync();

            var pagedResult = new PagedResult<BorrowRequestItem>(items, totalCount, criteria.Page, criteria.RowsPerPage);
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

        #region Client
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

        public async Task<BorrowRequestItem> GetBorrowDetailById(Guid borrowRequestId)
        {
            var query = from doc in _dbContext.Documents
                        join docB in _dbContext.DocumentBranchs
                            on doc.Id equals docB.DocumentId
                        join branch in _dbContext.Branchs
                            on docB.BranchId equals branch.Id

                        join borrow in _dbContext.BorrowRequest
                            on doc.Id equals borrow.DocumentId

                        join user in _dbContext.Users
                        on borrow.BorrowerId equals user.Id

                        where !doc.IsDeleted
                             && !borrow.IsDeleted
                             && borrow.Id == borrowRequestId
                        select new { doc, user, branch, borrow };

            var selectedItem = await query.FirstOrDefaultAsync();
            if (selectedItem == null)
            {
                return null;
            }


            var borrowItem = new BorrowRequestItem
            {
                BrrowRequestId = selectedItem.borrow.Id,
                DocumentId = selectedItem.doc.Id,
                BranchId = selectedItem.branch.Id,
                BranchName = selectedItem.branch.Name,
                BorrowerId = selectedItem.borrow.BorrowerId,
                BorrowerName = selectedItem.user.FullName,
                DocumentTitle = selectedItem.doc.Title,
                DocumentType = selectedItem.doc.DocumentType,
                DocumentStatus = selectedItem.doc.DocumentStatus,
                DocumentDescription = selectedItem.doc.Description,
                CoverImageUrl = selectedItem.doc.CoverImageUrl,
                PublishDate = selectedItem.doc.PublishDate,
                BorrowStatus = selectedItem.borrow.BorrowStatus,
                RequestDate = selectedItem.borrow.BorrowDate.Value,
                ReturnDate = selectedItem.borrow.ReturnDate.Value,
            };

            return borrowItem;

        }

        #endregion
    }
}
