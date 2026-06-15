using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class ResultData<T> : IResultData<T>
    {
        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public static ResultData<T> SuccessData(string message, T value) => new() { IsSuccess = true, Data = value, Message = message };
        public static ResultData<T> Failed(string message) => new() { IsSuccess = false, Message = message };
    }
}
