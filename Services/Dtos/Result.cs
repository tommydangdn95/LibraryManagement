using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class Result : IResult
    {
        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }
        public static Result Success(string message) => new() { IsSuccess = true, Message = message };
        public static Result Failed(string message) => new() { IsSuccess = false, Message = message };
    }
}
