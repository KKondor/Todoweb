using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Repositories
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync();
    }
}
