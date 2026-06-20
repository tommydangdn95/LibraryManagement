using Services.Enums;

namespace Services.ViewModels._BorrowViewModels
{
    public class GetListBorrowRequest
    {
        public int? BorrowStatus { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
