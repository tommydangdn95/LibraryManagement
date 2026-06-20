using Services.Enums;

namespace Services.Models.Criterias
{
    public class GetListBorrowRequestItemCriteria
    {
        public List<BorrowStatus> BorrowStatuses { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
