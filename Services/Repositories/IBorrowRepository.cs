using Services.Dtos;
using Services.Dtos.ApplicationDtos._Borrow;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;

namespace Services.Repositories
{
    public interface IBorrowRepository
    {
        public Task<bool> CreateAsync(BorrowRequest borrowRequest);
        public Task<bool> UpdateAsync(BorrowRequest borrowRequest);
        public Task<bool> DeleteAsync(Guid borrowRequestId, Guid submitUserId);
        public Task<BorrowRequest> GetById(Guid borrowRequestId);
        public Task<BorrowRequestItem> GetBorrowDetailById(Guid borrowRequestId);
        public Task<PagedResult<BorrowRequestItem>> GetBorrowRequestItem(GetListBorrowRequestItemCriteria criteria);

        #region Client
        public Task<PagedResult<DocumentItem>> GetBorrowDocumentByUser(GetDocumentListBorrowItemCriteria criteria, Guid userId);
        public Task<int> GetBorrowCount(BorrowStatus borrowStatus);
        #endregion
    }
}
