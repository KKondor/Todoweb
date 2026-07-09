using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Service
{
    public interface ITodoService
    {
        Task<List<TodoItemDto>> GetAllAsync();
        Task<List<TodoItemDto>> GetCompletedAsync();
        Task<List<TodoItemDto>> GetNotCompletedAsync();
        Task<List<TodoItemDto>> GetPriorityAsync(Todo.Priority priority);
        Task<TodoItemDto?> GetTodoByIdAsync(int id);
        Task<TodoItemDto> CreateTodoAsync(TodoItemDto todo);
        Task<TodoItemDto?> UpdateTodoAsync(int id, TodoItemDto todo);
        Task<bool> DeleteTodoAsync(int id);
        Task<TodoItemDto?> PatchTodoAsync(int id, TodoPatchDto inputTodo);
    }
}
