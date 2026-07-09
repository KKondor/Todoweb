using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Repositories
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync();
        Task<List<Todo>> GetCompletedAsync();
        Task<List<Todo>> GetNotCompletedAsync();
        Task<List<Todo>> GetPriorityAsync(Todo.Priority todoPriority);
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task<Todo?> UpdateTodoAsync(int id,Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}
