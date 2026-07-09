using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Todoweb.Backend.Model.API;
using Todoweb.Backend.Repositories;
using Todoweb.Backend.Service;

namespace Todoweb.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetAllAsync_ForTodos_ReturnsListTodos()
        {
            List<Todo> faketodos = new List<Todo> { new Todo { Id = 1, Name = "Test Todo" } };
            var mockrepo = new Mock<ITodoRepository>();
            mockrepo.Setup(r => r.GetAllAsync()).ReturnsAsync(faketodos);
            var service = new TodoService(mockrepo.Object);

            var result = await service.GetAllAsync();

            Assert.IsType<List<TodoItemDto>>(result);
        }

        [Fact]
        
        public async Task GetTaskByIdASync_Returns_Null_IfInvalid()
        {
            Todo faketodo =  new Todo { Id = 1, Name = "Test Todo" };
            var mockrepo = new Mock<ITodoRepository>();
            int id = 999;
            mockrepo.Setup(r => r.GetTodoByIdAsync(id)).ReturnsAsync((Todo?)null);
            var service = new TodoService(mockrepo.Object);

            var result = await service.GetTodoByIdAsync(id);

            Assert.Null(result);
        }
        [Fact]
        public async Task GetTaskByIdASync_Returns_NotNull_IfValid()
        {
            Todo faketodo = new Todo { Id = 1, Name = "Test Todo" };
            var mockrepo = new Mock<ITodoRepository>();
            int id = 1;
            mockrepo.Setup(r => r.GetTodoByIdAsync(id)).ReturnsAsync(faketodo);
            var service = new TodoService(mockrepo.Object);

            var result = await service.GetTodoByIdAsync(id);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetCompleteAsync_Returns_NotNull_IfCompleted()
        {
            List<Todo> faketodos = new List<Todo> { new Todo { Id = 1, Name = "Test Todo", IsComplete = true } };
            var mockrepo = new Mock<ITodoRepository>();
            mockrepo.Setup(r => r.GetCompletedAsync()).ReturnsAsync(faketodos);
            var service = new TodoService(mockrepo.Object);

            var result = await service.GetCompletedAsync();

            Assert.NotNull(result);
        }
    }
}
