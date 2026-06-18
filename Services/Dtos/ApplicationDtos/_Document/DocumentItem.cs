using Services.Enums;

namespace Services.Dtos.ApplicationDtos._Document
{
    public class DocumentItem
    {
        public Guid DocumentId { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public string DocumentTitle { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentDescription { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public BorrowStatus? BorrowStatus { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
