using Services.Dtos;

namespace Services.ViewModels._BorrowViewModels
{
    public class BorrowRequestList
    {
        public List<BorrowRequestViewItem> Items { get; set; }
        public Paging Paging { get; set; }
        public BorrowRequestList()
        {
            this.Items = new List<BorrowRequestViewItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
