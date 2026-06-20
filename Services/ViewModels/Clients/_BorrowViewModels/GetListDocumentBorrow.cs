using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels.Clients._BorrowViewModels
{
    public class GetListDocumentBorrow
    {
        public string Title { get; set; }
        public int? BorrowStatus { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
