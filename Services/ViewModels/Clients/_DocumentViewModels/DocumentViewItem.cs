using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels.Clients._DocumentViewModels
{
    public class DocumentViewItem
    {
        public Guid DocumentId { get; set; }
        public string Branch { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentDescription { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime PublishDate { get; set; }

        public string FormatPublishDate
        {
            get
            {
                return PublishDate.ToString("MMM yyyy");
            }
        }

        public bool IsAvaiableBorrowRequest
        {
            get
            {
                return (DocumentStatus == DocumentStatus.Available) &&
                    (BorrowStatus == Enums.BorrowStatus.Cancel || BorrowStatus == Enums.BorrowStatus.Returned);
            }
        }
        public BorrowStatus? BorrowStatus { get; set; }
        public string FormatDocumentStatus { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
