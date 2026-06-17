using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels._DocumentViewModels
{
    public class UpdateDocument
    {
        public Guid DocumenId { get; set; }
        public string Title { get; set; }
        public Guid BranchId { get; set; }
        public List<SelectListItem> ListBranches { get; set; }
        public int DocumentTypeId { get; set; }
        public List<SelectListItem> ListDocumentType { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int? DocumentStatus { get; set; }
        public DateTime PublishDate { get; set; }

        public UpdateDocument()
        {
            this.ListBranches = new List<SelectListItem>();
            this.ListDocumentType = new List<SelectListItem>();
        }
    }
}
