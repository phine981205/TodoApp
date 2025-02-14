using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Common
{
    public class ErrorHelper: IErrorHelper
    {
        public ErrorHelper()
        {

        }

        public Error InitializeBadRequestError(List<Messages> messages, ErrorCode errorCode)
        {
            var error = new Error
            {
                StatusCode = 400,
                Name = "BadRequest",
                Messages = messages,
                Code = errorCode.ToString(),
                Status = 400
            };

            return error;
        }

        public Error InitializeInternalServerError()
        {
            var error = new Error
            {
                StatusCode = 500,
                Name = "InternalServerError",
                Messages = new List<Messages> { new Messages { Message = "An unexpected error occurred. Please try again later." } },
                Code = ErrorCode.INTERNAL_SERVER_ERROR.ToString(),
                Status = 500
            };

            return error;
        }
    }
}
