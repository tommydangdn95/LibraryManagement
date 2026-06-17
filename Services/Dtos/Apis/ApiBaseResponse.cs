using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Apis
{
    public class ApiBaseResponse
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public int StatusCode { get; private set; }
        public ApiBaseResponse(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public ApiBaseResponse(bool isSuccess, string message, int statusCode) : this(isSuccess)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }
    }
}
