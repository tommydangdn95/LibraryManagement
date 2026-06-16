using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Criterias
{
    public class GetDocumentItemCriteria
    {
        public Guid? BranchId { get; set; }
        public string SearchDocumentName { get; set; }
        public int? DocumentType { get; set; }
        public int? DocumentStatus { get; set; }
        public int? BorrowStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; }
        public int RowsPerPage { get; set; }
    }
}
