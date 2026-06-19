using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Criterias
{
    public class GetDocumentListBorrowItemCriteria
    {
        public string Title { get;set;  }
        public List<BorrowStatus> BorrowStatuses { get; set; }
        public int Page { get; set; }
        public int RowsPerPage { get; set; }
    }
}
