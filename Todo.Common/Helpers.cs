using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Common
{
    public class Helpers
    {
        public Error InitializeNotFoundError()
        {
            var error = new Error
            {
                StatusCode = 400,
                Name = "NotFound",
                Messages = new List<Messages> { new Messages { Message = "Item not found." } },
                Code = ErrorCode.ITEM_NOT_FOUND.ToString(),
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

        public Error InitializeValidationError(List<Messages> messages)
        {
            var error = new Error
            {
                StatusCode = 422,
                Name = "ValidationError",
                Messages = messages,
                Code = ErrorCode.VALIDATION_ERROR.ToString(),
                Status = 400
            };

            return error;
        }
    }
}
