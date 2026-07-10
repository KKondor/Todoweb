using Microsoft.EntityFrameworkCore;
using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDb _todoDb;
        public TodoRepository(TodoDb todoDb) => _todoDb = todoDb;

        public async Task<List<Todo>> GetAllAsync() => await _todoDb.Todos.ToListAsync();
        public async Task<List<Todo>> GetFilteredAsync(string? name, bool? isComplete, Todo.Priority? priority)
        {
            var query = _todoDb.Todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(t => t.Name.Contains(name));
            if (isComplete.HasValue)
                query = query.Where(t => t.IsComplete == isComplete.Value);
            if (priority.HasValue)
                query = query.Where(t => t.TodoPriority == priority.Value);

            return await query.ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(int id) => await _todoDb.Todos.FindAsync(id);
        public async Task<Todo> CreateTodoAsync(Todo todo) {
            var result = await _todoDb.Todos.AddAsync(todo);
            await _todoDb.SaveChangesAsync();
            return todo;
        }
        public async Task<Todo?> UpdateTodoAsync(int id, Todo todo)
        {
            var foundTodo = await _todoDb.Todos.FindAsync(id);
            if (foundTodo is null) return null;

            foundTodo.Name = todo.Name;
            foundTodo.IsComplete = todo.IsComplete;
            foundTodo.TodoPriority = todo.TodoPriority;
            foundTodo.Description = todo.Description;
            foundTodo.DueDate = todo.DueDate;

            await _todoDb.SaveChangesAsync();

            return foundTodo;
        }
        public async Task<bool> DeleteTodoAsync(int id)
        {
            var foundTodo = await _todoDb.Todos.FindAsync(id);
            if (foundTodo is null) return false;
            _todoDb.Todos.Remove(foundTodo);
            await _todoDb.SaveChangesAsync();
            return true;
        }
        
        
    }
}
