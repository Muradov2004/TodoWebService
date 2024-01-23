using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Todo
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItemDto> ChangeTodoItemStatus(ChangeStatusRequest request)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == request.Id);
            if (todoItem is not null)
            {
                todoItem.IsCompleted = request.IsCompeleted;
                todoItem.UpdatedTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return new(todoItem.Id, todoItem.Text, todoItem.IsCompleted, todoItem.CreatedTime);
            }
            else return null!;

        }

        public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request)
        {
            var todo = new TodoItem() { Text = request.Text, IsCompleted = false, UpdatedTime = DateTime.Now, CreatedTime = DateTime.Now };
            await _context.TodoItems.AddAsync(todo);
            await _context.SaveChangesAsync();
            var lastItem = await _context.TodoItems.OrderBy(t => t.Id).LastAsync();
            return new TodoItemDto(lastItem.Id, lastItem.Text, lastItem.IsCompleted, lastItem.CreatedTime);
        }

        public async Task<bool> DeleteTodo(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<TodoItemDto>> GetAll(int page, int pageSize)
        {
            var todoList = await _context.TodoItems.ToListAsync();

            var startIndex = page * pageSize;
            var endIndex = startIndex + pageSize;

            endIndex = Math.Min(endIndex, todoList.Count);

            var dtoList = new List<TodoItemDto>();

            for (int i = startIndex; i < endIndex; i++)
            {
                dtoList.Add(new TodoItemDto(
                    Id: todoList[i].Id,
                    Text: todoList[i].Text,
                    IsCompleted: todoList[i].IsCompleted,
                    CreatedTime: todoList[i].CreatedTime
                ));
            }

            return dtoList;
        }


        public async Task<TodoItemDto?> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            return todoItem is not null
                ? new TodoItemDto(
                    Id: todoItem.Id,
                    Text: todoItem.Text,
                    IsCompleted: todoItem.IsCompleted,
                    CreatedTime: todoItem.CreatedTime)
                : null;
        }
    }
}
