using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels._BorrowViewModels
{
    public class UpdateBorrowRequest
    {
        public Guid BorrowRequestId { get; set; }
        public Guid SubmitedUserId { get; set; }
        public int BorrowStatus { get; set; }
    }
}
