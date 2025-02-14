using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Common
{
    public class ServiceResponse<T>
    {
        public Result Result { get; private set; } = Result.Succeeded;
        public T Data { get; set; }
        public Error Error { get; private set; }

        public void AddError(Error error)
        {
            Result = Result.Failed;
            Error = error;
        }
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Name { get; set; }
        public List<Messages> Messages { get; set; }
        //public string Message { get; set; }
        //public Dictionary<string, string> Messages { get; set; } = null;
        public string Code { get; set; }
        public int Status { get; set; }
    }


    public enum Result
    {
        Failed = 0,
        Succeeded = 1
    }

    public class Messages
    {
        public string Message { get; set; }
        public string FieldName { get; set; }
    }

}
