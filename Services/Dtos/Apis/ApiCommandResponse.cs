using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Apis
{
    public class ApiCommandResponse : ApiBaseResponse
    {
        public ApiCommandResponse(string message, int statusCode)
            : base(true, message, statusCode)
        {

        }
    }
}
