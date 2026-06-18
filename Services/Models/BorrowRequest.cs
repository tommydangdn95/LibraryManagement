using Services.Enums;
using Services.Models._Users;

namespace Services.Models
{
    public class BorrowRequest : BaseModel
    {
        public Guid BorrowBranchId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid BorrowerId { get; set; }
        public Guid ReturnBranchId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus BorrowStatus { get; set; }
        public string Note { get; set; }
        public Guid ApprovedUserId { get; set; }
    }
}
