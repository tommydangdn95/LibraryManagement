namespace Services.Dtos
{
    public class ListDto<T>
    {
        public IEnumerable<T> Data { get; set; }
        public Paging Paging { get; set; }
    }
}
