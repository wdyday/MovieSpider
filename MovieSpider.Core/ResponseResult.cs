using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseResult(bool success = true, string message = null, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
