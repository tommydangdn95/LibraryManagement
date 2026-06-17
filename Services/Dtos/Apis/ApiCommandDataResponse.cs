using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Apis
{
    public class ApiCommandDataResponse<T> : ApiBaseResponse
    {
        public T Data { get; private set; }
        public ApiCommandDataResponse(string message, int statusCode, T data)
            : base(true, message, statusCode)
        {
            this.Data = data;
        }
    }
}
