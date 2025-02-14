using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Models;
using Todo.Common;

namespace Todo.Application.Service
{
    public interface ITodoService
    {
        Task<ServiceResponse<bool>> CreateTodoItemAsync(UpsertTodoItemRequest dto, string userId);
        Task<ServiceResponse<bool>> UpdateTodoItemAsync(UpsertTodoItemRequest dto, int todoItemId, string userId);
        Task<ServiceResponse<bool>> MarkTodoItemAsCompleteAsync(int todoItemId, string userId);
        Task<ServiceResponse<bool>> DeleteTodoAsync(int id, string userId);
        Task<ServiceResponse<GetTodoItemResponse>> GetTodoItemByIdAsync(int todoItemId);
        Task<ServiceResponse<List<GetTodoItemResponse>>> GetTodoItemListAsync(string userId);
    }
}
