using Services.Dtos;
using Services.ViewModels.Clients._BorrowViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Applications
{
    public interface IBorrowService
    {
        public Task<IResult> CreateBorrowRequest(CreateBorrowRequest request, Guid submitUserId);
        public Task<IResultData<DocumentViewItem>>()
    }
}
