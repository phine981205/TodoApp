using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Models;
using Todo.Application.Models.Validators;
using Todo.Common;
using Todo.Core.Domain;
using Todo.Infrastructure;

namespace Todo.Application.Service
{
    public class TodoService: ITodoService
    {
        private readonly TodoDbContext _todoDbContext;
        private readonly ILogger<TodoService> _logger;
        private readonly IErrorHelper _errorHelper;

        public TodoService(
            TodoDbContext todoDbContext, 
            ILogger<TodoService> logger,
            IErrorHelper errorHelper)
        {
            _logger = logger;
            _errorHelper = errorHelper;
            _todoDbContext = todoDbContext;
        }

        public async Task<ServiceResponse<bool>> CreateTodoItemAsync(UpsertTodoItemRequest dto, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            var validator = new UpsertTodoItemValidator();
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => new Messages { Message = e.ErrorMessage })
                    .ToList();

                var error = _errorHelper.InitializeBadRequestError(errorMessages, ErrorCode.VALIDATION_ERROR);

                serviceResponse.AddError(error);
                return serviceResponse;
            }

            try
            {
                var todoItem = new TodoItem
                {
                    Title = dto.Title,
                    UserId = userId,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _todoDbContext.TodoItems.AddAsync(todoItem);
                await _todoDbContext.SaveChangesAsync();

                serviceResponse.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating To-Do item");

                var error = _errorHelper.InitializeInternalServerError();

                serviceResponse.AddError(error);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateTodoItemAsync(UpsertTodoItemRequest dto, int todoItemId, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            var existingTodoItem = await _todoDbContext.TodoItems
                .Where(x => x.Id == todoItemId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingTodoItem == null)
            {
                var message = new List<Messages> { new Messages { Message = "Item not found." } };

                var error = _errorHelper.InitializeBadRequestError(message, ErrorCode.ITEM_NOT_FOUND);

                serviceResponse.AddError(error);

                return serviceResponse;
            }

            var validator = new UpsertTodoItemValidator();
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => new Messages { Message = e.ErrorMessage })
                    .ToList();

                var error = _errorHelper.InitializeBadRequestError(errorMessages, ErrorCode.VALIDATION_ERROR);

                serviceResponse.AddError(error);
                return serviceResponse;
            }

            try
            {
                existingTodoItem.Title = dto.Title;
                existingTodoItem.ModifiedBy = userId;
                existingTodoItem.ModifiedAt = DateTime.UtcNow;

                _todoDbContext.TodoItems.Update(existingTodoItem);
                await _todoDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating To-Do item");

                var error = _errorHelper.InitializeInternalServerError();
                serviceResponse.AddError(error);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> MarkTodoItemAsCompleteAsync(int todoItemId, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            var existingTodoItem = await _todoDbContext.TodoItems
                .Where(x => x.Id == todoItemId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingTodoItem == null)
            {
                var message = new List<Messages> { new Messages { Message = "Item not found." } };
                var error = _errorHelper.InitializeBadRequestError(message, ErrorCode.ITEM_NOT_FOUND);

                serviceResponse.AddError(error);

                return serviceResponse;
            }

            try
            {
                existingTodoItem.IsCompleted = true;
                existingTodoItem.ModifiedBy = userId;
                existingTodoItem.ModifiedAt = DateTime.UtcNow;

                _todoDbContext.TodoItems.Update(existingTodoItem);
                await _todoDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while mark To-Do item as completed.");

                var error = _errorHelper.InitializeInternalServerError();

                serviceResponse.AddError(error);
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteTodoAsync(int todoItemId, string userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            var existingTodoItem = await _todoDbContext.TodoItems
                .Where(x => x.Id == todoItemId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingTodoItem == null)
            {
                var message = new List<Messages> { new Messages { Message = "Item not found." } };
                var error = _errorHelper.InitializeBadRequestError(message, ErrorCode.ITEM_NOT_FOUND);

                serviceResponse.AddError(error);

                return serviceResponse;
            }

            try
            {
                existingTodoItem.IsDeleted = true;
                existingTodoItem.ModifiedBy = userId;
                existingTodoItem.ModifiedAt = DateTime.UtcNow;

                _todoDbContext.TodoItems.Update(existingTodoItem);
                await _todoDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while delete To-Do item.");

                var error = _errorHelper.InitializeInternalServerError();

                serviceResponse.AddError(error);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetTodoItemResponse>> GetTodoItemByIdAsync(int todoItemId)
        {
            var serviceResponse = new ServiceResponse<GetTodoItemResponse>();

            var todoItem = await _todoDbContext.TodoItems
                .Where(x => x.Id == todoItemId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (todoItem == null)
            {
                var message = new List<Messages> { new Messages { Message = "Item not found." } };
                var error = _errorHelper.InitializeBadRequestError(message, ErrorCode.ITEM_NOT_FOUND);

                serviceResponse.AddError(error);

                return serviceResponse;
            }

            var result = new GetTodoItemResponse
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                IsCompleted = todoItem.IsCompleted,
                LastUpdateddAt = todoItem.ModifiedAt ?? todoItem.CreatedAt
            };

            serviceResponse.Data = result;

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetTodoItemResponse>>> GetTodoItemListAsync(string userId)
        {
            var serviceResponse = new ServiceResponse<List<GetTodoItemResponse>>();

            var result = await _todoDbContext.TodoItems
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => new GetTodoItemResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsCompleted = x.IsCompleted
                })
                .ToListAsync();

            serviceResponse.Data = result;

            return serviceResponse;
        }
    }
}
