using Services.Dtos;

namespace Services.ViewModels._BorrowViewModels
{
    public class BorrowRequestList
    {
        public List<BorrowRequestViewItem> Items { get; set; }
        public int? BorrowStatus { get; set; }
        public Paging Paging { get; set; }

        public bool IsActiveNewRequestTab
        {
            get
            {
                return BorrowStatus.HasValue && BorrowStatus.Value == (int)Enums.BorrowStatus.SubmitRequest;
            }
        }

        public bool IsActiveApprovedTab
        {
            get
            {
                return BorrowStatus.HasValue && BorrowStatus.Value == (int)Enums.BorrowStatus.Approved;
            }
        }

        public bool IsActiveBorrowingTab
        {
            get
            {
                return BorrowStatus.HasValue && BorrowStatus.Value == (int)Enums.BorrowStatus.Borrowing;
            }
        }


        public bool IsActiveCancelTab
        {
            get
            {
                return BorrowStatus.HasValue && BorrowStatus.Value == (int)Enums.BorrowStatus.Cancel;
            }
        }

        public bool IsActiveReturnedTab
        {
            get
            {
                return BorrowStatus.HasValue && BorrowStatus.Value == (int)Enums.BorrowStatus.Returned;
            }
        }

        public bool IsActiveAllTab
        {
            get
            {
                return !BorrowStatus.HasValue;
            }
        }
        public BorrowRequestList()
        {
            this.Items = new List<BorrowRequestViewItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
