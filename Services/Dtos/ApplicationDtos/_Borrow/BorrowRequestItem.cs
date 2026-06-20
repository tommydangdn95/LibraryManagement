using Services.Enums;

namespace Services.Dtos.ApplicationDtos._Borrow
{
    public class BorrowRequestItem
    {
        public Guid BrrowRequestId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentDescription { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public Guid BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public BorrowStatus BorrowStatus { get; set; }
        public DateTime PublishDate { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
