using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels._BorrowViewModels
{
    public class BorrowRequestViewItem
    {
        public Guid BrrowRequestId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentDescription { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string BranchName { get; set; }
        public string BorrowerName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public BorrowStatus BorrowStatus { get; set; }
        public DateTime PublishDate { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
