using Microsoft.EntityFrameworkCore;

namespace Todoweb.Backend.Model.API
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}
