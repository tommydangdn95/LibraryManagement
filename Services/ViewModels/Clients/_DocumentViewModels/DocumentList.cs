using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;

namespace Services.ViewModels.Clients._DocumentViewModels
{
    public class DocumentList
    {
        public List<DocumentViewItem> Items { get; set; }
        public Paging Paging { get; set; }
        public DocumentList()
        {
            this.Items = new List<DocumentViewItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
