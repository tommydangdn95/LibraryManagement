using Services.Dtos;

namespace Services.ViewModels._BranchViewModels
{
    public class BranchList
    {
        public List<BranchItem> Items { get; set; }
        public Paging Paging { get; set; }
        public BranchList()
        {
            this.Items = new List<BranchItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
