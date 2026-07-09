using Todoweb.Backend.Model.API;
using Todoweb.Backend.Repositories;

namespace Todoweb.Backend.Service
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;
        public TodoService(ITodoRepository repo) => _repo = repo;
        public async Task<List<TodoItemDto>> GetAllAsync()
        {
            var todos = await _repo.GetAllAsync();
            return todos.Select(x => new TodoItemDto(x)).ToList();
        }
        public async Task<List<TodoItemDto>> GetCompletedAsync()
        {
            var completedTodos = await _repo.GetCompletedAsync();
            return completedTodos.Select(x => new TodoItemDto(x)).ToList();
        }

        public async Task<List<TodoItemDto>> GetNotCompletedAsync()
        {
            var completedTodos = await _repo.GetNotCompletedAsync();
            return completedTodos.Select(x => new TodoItemDto(x)).ToList();
        }

        public async Task<List<TodoItemDto>> GetPriorityAsync(Todo.Priority priority)
        {
            var priorityTodos = await _repo.GetPriorityAsync(priority);
            return priorityTodos.Select(x => new TodoItemDto(x)).ToList();
        }
        public async Task<TodoItemDto?> GetTodoByIdAsync(int id)
        {
            var foundTodo = await _repo.GetTodoByIdAsync(id);
            return foundTodo == null ? null : new TodoItemDto(foundTodo);
        }
        public async Task<TodoItemDto> CreateTodoAsync(TodoItemDto todo)
        {
            Todo convertedTodo = new Todo
            {
                Id = todo.Id,
                Name = todo.Name,
                TodoPriority = todo.TodoPriority,
                Description = todo.Description,
                IsComplete = todo.IsComplete,
                DueDate = todo.DueDate,
            };
            var created = await _repo.CreateTodoAsync(convertedTodo);
            return new TodoItemDto(created);
        }
        public async Task<TodoItemDto?> UpdateTodoAsync(int id, TodoItemDto todo)
        {
            Todo convertedTodo = new Todo
            {
                Id = todo.Id,
                Name = todo.Name,
                TodoPriority = todo.TodoPriority,
                Description = todo.Description,
                IsComplete = todo.IsComplete,
                DueDate = todo.DueDate,
            };
            var updated = await _repo.UpdateTodoAsync(id,convertedTodo);
            if (updated == null) return null;
            return new TodoItemDto(updated);
        }
        public async Task<bool> DeleteTodoAsync(int id)
        {
            bool result = await _repo.DeleteTodoAsync(id);
            return result;
        }
        public async Task<TodoItemDto?> PatchTodoAsync(int id, TodoPatchDto inputTodo)
        {
            var foundTodo = await _repo.GetTodoByIdAsync(id);
            if (foundTodo is null) return null;

            if (inputTodo.Name is not null) foundTodo.Name = inputTodo.Name;
            if (inputTodo.IsComplete is not null) foundTodo.IsComplete = inputTodo.IsComplete.Value;
            if (inputTodo.Description is not null) foundTodo.Description = inputTodo.Description;
            if (inputTodo.TodoPriority is not null) foundTodo.TodoPriority = inputTodo.TodoPriority.Value;
            if (inputTodo.DueDate is not null) foundTodo.DueDate = inputTodo.DueDate;

            var result = await _repo.UpdateTodoAsync(id,foundTodo);
            if (result is null) return null;
            return new TodoItemDto(result);
        }
    }
}
