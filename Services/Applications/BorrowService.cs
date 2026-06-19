using Services.Dtos;
using Services.Models;
using Services.Repositories;
using Services.ViewModels.Clients._BorrowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
