using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Apis
{
    public class ApiErrorResponse<T> : ApiBaseResponse
    {
        public ApiErrorResponse() : base(false)
        {

        }

        public ApiErrorResponse(string message, int statusCode) :
            base(false, message, statusCode)
        {

        }
    }
}
