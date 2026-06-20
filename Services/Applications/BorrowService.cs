using Services.Dtos;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;
using Services.Repositories;
using Services.Utils;
using Services.ViewModels.Clients._BorrowViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    }
}
