using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Common
{
    public interface IErrorHelper
    {
        Error InitializeBadRequestError(List<Messages> messages, ErrorCode errorCode);
        Error InitializeInternalServerError();
    }
}
