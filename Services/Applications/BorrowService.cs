using Services.Dtos;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;
using Services.Repositories;
using Services.Utils;
using Services.ViewModels._BorrowViewModels;
using Services.ViewModels.Clients._BorrowViewModels;

namespace Services.Applications
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        public BorrowService(IBorrowRepository borrowRepository)
        {
            this._borrowRepository = borrowRepository;
        }
        public async Task<IResult> CreateBorrowRequest(CreateBorrowRequest request, Guid submitUserId)
        {
            var borrowRequest = new BorrowRequest()
            {
                DocumentId = request.DocumentId,
                BorrowBranchId = request.BranchId,
                BorrowDate = DateTime.Now,
                BorrowerId = submitUserId,
                BorrowStatus = Enums.BorrowStatus.SubmitRequest,
                Note = request.Note,
                CreatedDate = DateTime.Now,
                CreatedBy = submitUserId,
                ReturnDate = request.ReturnDate,
            };

            var result = await _borrowRepository.CreateAsync(borrowRequest);
            if (!result)
            {
                return Result.Failed("Create new borrow request failed");
            }

            return Result.Success("Create new borrow request successfully");
        }

        public async Task<IResultData<DocumentBorrowListItem>> GetBorrowDocumentByUser(GetListDocumentBorrow request, Guid userId)
        {
            var listBorrowStatus = new List<BorrowStatus>();
            if (request.BorrowStatus.HasValue)
            {
                var parseStatus = request.BorrowStatus.Value.ToEnum<BorrowStatus>();
                listBorrowStatus.Add(parseStatus.Value);
            }

            var criteria = new GetDocumentListBorrowItemCriteria()
            {
                Title = request.Title,
                BorrowStatuses = listBorrowStatus,
                Page = request.Page,
                RowsPerPage = request.RowsPerPage
            };

            var pageResult = await _borrowRepository.GetBorrowDocumentByUser(criteria, userId);
            var documentBorrowListItem = new DocumentBorrowListItem()
            {
                Items = pageResult.Items.Select(x => new DocumentBorrowViewItem()
                {
                    DocumentId = x.DocumentId,
                    Branch = x.BranchName,
                    DocumentTitle = x.DocumentTitle,
                    DocumentDescription = x.DocumentDescription,
                    DocumentStatus = x.DocumentStatus,
                    CoverImageUrl = x.CoverImageUrl,
                    DocumentType = x.DocumentType.ToString(),
                    BorrowStatus = x.BorrowStatus,
                    BorrowDate = x.BorrowDate,
                    ReturnDate = x.ReturnDate
                }).ToList(),

                Paging = Paging.GetPaging(request.Page, request.RowsPerPage, pageResult.TotalCount)
            };

            return ResultData<DocumentBorrowListItem>.SuccessData("Get list item successfully", documentBorrowListItem);
        }

        public async Task<IResultData<BorrowRequestList>> GetBorrowRequestList(GetListBorrowRequest request)
        {
            var listBorrowStatus = new List<BorrowStatus>();
            if (request.BorrowStatus.HasValue)
            {
                var parseStatus = request.BorrowStatus.Value.ToEnum<BorrowStatus>();
                listBorrowStatus.Add(parseStatus.Value);
            }

            var criteria = new GetListBorrowRequestItemCriteria()
            {
                BorrowStatuses = listBorrowStatus,
                Page = request.Page,
                RowsPerPage = request.RowsPerPage
            };

            var pageResult = await this._borrowRepository.GetBorrowRequestItem(criteria);
            var borrowRequestListItem = new BorrowRequestList()
            {
                Items = pageResult.Items.Select(x => new BorrowRequestViewItem()
                {
                    BorrowRequestId = x.BrrowRequestId,
                    DocumentTitle = x.DocumentTitle,
                    DocumentDescription = x.DocumentDescription,
                    DocumentStatus = x.DocumentStatus,
                    CoverImageUrl = x.CoverImageUrl,
                    DocumentType = x.DocumentType,
                    BorrowStatus = x.BorrowStatus,
                    RequestDate = x.RequestDate,
                    ReturnDate = x.ReturnDate,
                    BorrowerName = x.BorrowerName,
                    BranchName = x.BranchName,
                    PublishDate = x.PublishDate,
                }).ToList(),

                Paging = Paging.GetPaging(request.Page, request.RowsPerPage, pageResult.TotalCount)
            };

            return ResultData<BorrowRequestList>.SuccessData("Get list item successfully", borrowRequestListItem);
        }

        public async Task<IResultData<BorrowRequestViewItem>> GetDetailBorrowRequestItem(Guid borrowRequestItem)
        {
            var borrowRequest = await _borrowRepository.GetBorrowDetailById(borrowRequestItem);
            if (borrowRequest == null)
            {
                return ResultData<BorrowRequestViewItem>.Failed("Could not get borrow request item");
            }

            var borrowRequestViewItem = new BorrowRequestViewItem()
            {
                BorrowRequestId = borrowRequest.BrrowRequestId,
                DocumentTitle = borrowRequest.DocumentTitle,
                DocumentDescription = borrowRequest.DocumentDescription,
                DocumentStatus = borrowRequest.DocumentStatus,
                CoverImageUrl = borrowRequest.CoverImageUrl,
                DocumentType = borrowRequest.DocumentType,
                BorrowStatus = borrowRequest.BorrowStatus,
                RequestDate = borrowRequest.RequestDate,
                ReturnDate = borrowRequest.ReturnDate,
                BorrowerName = borrowRequest.BorrowerName,
                BranchName = borrowRequest.BranchName,
                PublishDate = borrowRequest.PublishDate,
            };

            return ResultData<BorrowRequestViewItem>.SuccessData("Get borrow item detail successfully", borrowRequestViewItem);

        }

        public async Task<IResult> UpdateBrrowStatus(UpdateBorrowRequest updateBorrowRequest)
        {
            var borrowRequest = await _borrowRepository.GetById(updateBorrowRequest.BorrowRequestId);
            if (borrowRequest == null)
            {
                return Result.Failed("Borrow Request Not Found");
            }

            var parseBorrowStatus = updateBorrowRequest.BorrowStatus.ToEnum<BorrowStatus>();
            if (parseBorrowStatus.HasValue)
            {
                borrowRequest.BorrowStatus = parseBorrowStatus.Value;
            }

            borrowRequest.ApprovedUserId = updateBorrowRequest.SubmitedUserId;
            borrowRequest.UpdatedBy = updateBorrowRequest.SubmitedUserId;
            borrowRequest.UpdatedDate = DateTime.Now;
            var result = await this._borrowRepository.UpdateAsync(borrowRequest);
            if (!result)
            {
                return Result.Failed("Update new borrow request failed");
            }

            return Result.Success("Create new borrow request successfully");
        }
    }
}
