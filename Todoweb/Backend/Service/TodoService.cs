using Todoweb.Backend.Model.API;
using Todoweb.Backend.Repositories;

namespace Todoweb.Backend.Service
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;
        public TodoService(ITodoRepository repo) => _repo = repo;
        public async Task<List<TodoItemDto>> GetAllASync()
        {
            var todos = await _repo.GetAllAsync();
            return todos.Select(x => new TodoItemDto(x)).ToList();
        }
    }
}
