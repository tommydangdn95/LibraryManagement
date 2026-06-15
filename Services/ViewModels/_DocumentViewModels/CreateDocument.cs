using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.ViewModels._DocumentViewModels
{
    public class CreateDocument
    {
        public string Title { get; set; }
        public Guid BranchId { get; set; }
        public List<SelectListItem> ListBranches { get; set; }
        public int DocumentTypeId { get; set; }
        public List<SelectListItem> ListDocumentType { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }

        public CreateDocument()
        {
            this.ListBranches = new List<SelectListItem>();
            this.ListDocumentType = new List<SelectListItem>();
        }
    }
}
