using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels.Clients._BorrowViewModels
{
    public class DocumentBorrowViewItem
    {
        public Guid DocumentId { get; set; }
        public string Branch { get; set; }
        public Guid BranchId { get; set; }
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
                return PublishDate.ToString("dd-MM-yyyy");
            }
        }

        public bool IsAllowCancelRequest
        {
            get
            {
                return this.BorrowStatus == Enums.BorrowStatus.SubmitRequest;
            }
        }
        public BorrowStatus? BorrowStatus { get; set; }
        public string FormatDocumentStatus { get; set; }
        public DateTime? BorrowDate { get; set; }

        public string FormatBorrowDate
        {
            get
            {
                if (BorrowDate.HasValue)
                {
                    return this.BorrowDate.Value.ToString("dd-MM-yyyy");
                }

                return "N/A";
            }
        }
        public DateTime? ReturnDate { get; set; }

        public string FormatReturnDate
        {
            get
            {
                if (ReturnDate.HasValue)
                {
                    return this.ReturnDate.Value.ToString("dd-MM-yyyy");
                }

                return "N/A";
            }
        }
    }
}
