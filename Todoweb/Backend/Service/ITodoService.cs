using Todoweb.Backend.Model.API;

namespace Todoweb.Backend.Service
{
    public interface ITodoService
    {
        Task<List<TodoItemDto>> GetAllASync();
    }
}
