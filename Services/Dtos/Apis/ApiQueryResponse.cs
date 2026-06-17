namespace Services.Dtos.Apis
{
    public class ApiQueryResponse<T> : ApiBaseResponse
    {
        public T Data { get; private set; }

        public ApiQueryResponse(string message, int statusCode, T data) : base(true, message, statusCode)
        {
            this.Data = data;
        }
    }
}
