using Microsoft.EntityFrameworkCore;
using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDb _todoDb;
        public TodoRepository(TodoDb todoDb) => _todoDb = todoDb;

        public async Task<List<Todo>> GetAllAsync() => await _todoDb.Todos.ToListAsync();
    }
}
