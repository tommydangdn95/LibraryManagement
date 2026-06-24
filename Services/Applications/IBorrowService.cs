using Services.Dtos;
using Services.Enums;
using Services.ViewModels._BorrowViewModels;
using Services.ViewModels.Clients._BorrowViewModels;

namespace Services.Applications
{
    public interface IBorrowService
    {
        public Task<IResultData<BorrowRequestList>> GetBorrowRequestList(GetListBorrowRequest request);
        public Task<IResult> UpdateBrrowStatus(UpdateBorrowRequest updateBorrowRequest);
        public Task<IResult> CreateBorrowRequest(CreateBorrowRequest request, Guid submitUserId);
        public Task<IResultData<BorrowRequestViewItem>> GetDetailBorrowRequestItem(Guid borrowRequestItem);
        public Task<IResultData<DocumentBorrowListItem>> GetBorrowDocumentByUser(GetListDocumentBorrow request, Guid userId);
        public Task<IResultData<BorrowRequestIndex>> GetBorrowStatusCount();
    }
}
