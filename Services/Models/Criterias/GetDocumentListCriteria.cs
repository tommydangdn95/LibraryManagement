namespace Services.Models.Criterias
{
    public class GetDocumentListCriteria
    {
        public string SearchName { get; set; }
        public Guid? BranchId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
