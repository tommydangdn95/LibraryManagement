using Services.Dtos;

namespace Services.ViewModels._UserViewModels
{
    public class UserList
    {
        public List<UserItem> Items { get; set; }
        public Paging Paging { get; set; }
        public UserList()
        {
            this.Items = new List<UserItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
