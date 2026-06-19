namespace Services.ViewModels.Clients._BorrowViewModels
{
    public class CreateBorrowRequest
    {
        public Guid DocumentId { get; set; }
        public string? DocumentTitle { get; set; }
        public string? DocumentType { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Note { get; set; }
    }
}
