using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.ViewModels._DocumentViewModels
{
    public class DocumentListAdminQuery
    {
        public string SearchName { get; set; }
        public Guid? BranchId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
