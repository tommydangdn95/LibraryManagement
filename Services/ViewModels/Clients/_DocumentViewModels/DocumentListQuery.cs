using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.ViewModels.Clients._DocumentViewModels
{
    public class DocumentListQuery
    {
        public Guid? BranchId { get; set; }
        public List<SelectListItem> BranchListItem { get; set; }
        public List<(int value, string label)> ListDocumentType { get; set; }
        public List<(int value, string label)> ListBorrowStatus { get; set;  } 
        public string SearchDocumentName { get; set; }
        public List<string> DocumentTypes { get; set; }
        public int? DocumentStatus { get; set; }
        public int? BorrowStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;

        public DocumentListQuery()
        {
            this.BranchListItem = new List<SelectListItem>();
            this.ListDocumentType = new List<(int, string)>();
            this.ListBorrowStatus = new List<(int, string)>();
            this.DocumentTypes = new List<string>();
        }
    }

    public class GetDocumentListQuery
    {
        public string BranchId { get; set; }
        public string SearchDocumentName { get; set; }
        public List<string> DocumentTypes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;

        public GetDocumentListQuery()
        {
            this.DocumentTypes = new List<string>();
        }

    }
}
