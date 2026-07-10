using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Repositories
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync();
        Task<List<Todo>> GetFilteredAsync(string? name, bool? isComplete, Todo.Priority? priority);
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task<Todo?> UpdateTodoAsync(int id,Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}
