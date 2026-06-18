using Services.Enums;

namespace Services.ViewModels._DocumentViewModels
{
    public class DocumentItemAdmin
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Branch { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime PublishDate { get; set; }

    }
}
