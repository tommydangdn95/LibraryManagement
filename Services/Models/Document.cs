using Services.Enums;

namespace Services.Models
{
    public class Document : BaseModel
    {
        public string Title { get; set; }
        public DocumentType DocumentType { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public ICollection<DocumentBranch> DocumentBranches { get; set; }
    }
}
