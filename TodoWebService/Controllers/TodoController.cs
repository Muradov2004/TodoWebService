using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Services.Todo;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            var item = await _todoService.GetTodoItem(id);


            return item is not null
                ? item
                : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            return await _todoService.DeleteTodo(id);
        }

        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
        {
            return await _todoService.CreateTodo(request);
        }

        [HttpPost("change-status")]
        public async Task<ActionResult<TodoItemDto>> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            return await _todoService.ChangeTodoItemStatus(request);
        }

        [HttpGet("get-all/{page}/{pageSize}")]
        public async Task<ActionResult<List<TodoItemDto>>> GetAll(int page, int pageSize)
        {
            return await _todoService.GetAll(page, pageSize);
        }
    }
}
