using Services.Dtos;

namespace Services.ViewModels._DocumentViewModels
{
    public class DocumentListAdmin
    {
        public List<DocumentItemAdmin> Items { get; set; }
        public Paging Paging { get; set; }
        public DocumentListAdmin()
        {
            this.Items = new List<DocumentItemAdmin>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
