using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Application.Models;
using Todo.Application.Service;
using Todo.Common;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly ITodoService _todoService;

        public TodoItemController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private string GetUserId() => "552eecfb-7e6c-4e35-a153-3e70d1a8d3a9"; // Hardcode userId since no implement Authentication

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] UpsertTodoItemRequest dto)
        {
            var serviceResponse = await _todoService.CreateTodoItemAsync(dto, GetUserId());

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }


        [HttpPut("{todoItemId}")]
        public async Task<IActionResult> UpdateTodo(int todoItemId, [FromBody] UpsertTodoItemRequest dto)
        {
            var serviceResponse = await _todoService.UpdateTodoItemAsync(dto, todoItemId, GetUserId());

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }

        [HttpPut("{todoItemId}/complete")]
        public async Task<IActionResult> MarkTodoAsComplete(int todoItemId)
        {
            var serviceResponse = await _todoService.MarkTodoItemAsCompleteAsync(todoItemId, GetUserId());

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }

        [HttpDelete("{todoItemId}")]
        public async Task<IActionResult> DeleteTodo(int todoItemId)
        {
            var serviceResponse = await _todoService.DeleteTodoAsync(todoItemId, GetUserId());

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }

        [HttpGet("{todoItemId}")]
        public async Task<IActionResult> GetTodoById(int todoItemId)
        {
            var serviceResponse = await _todoService.GetTodoItemByIdAsync(todoItemId);

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItemList()
        {
            var serviceResponse = await _todoService.GetTodoItemListAsync(GetUserId());

            if (serviceResponse.Result == Result.Failed)
            {
                return BadRequest(serviceResponse.Error);
            }

            return Ok(serviceResponse);
        }
    }
}
