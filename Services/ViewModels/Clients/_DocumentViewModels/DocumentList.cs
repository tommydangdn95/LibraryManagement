using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;

namespace Services.ViewModels.Clients._DocumentViewModels
{
    public class DocumentList
    {
        public List<DocumentItem> Items { get; set; }
        public Paging Paging { get; set; }
        public DocumentList()
        {
            this.Items = new List<DocumentItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
