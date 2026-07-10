using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using Todoweb.Backend.Model.API;
using Todoweb.Backend.Model.Helpers;
using Todoweb.Backend.Service;

namespace Todoweb.Backend.Model.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this IEndpointRouteBuilder group)
        {
            group.MapGet("/", GetAllTodos);
            group.MapGet("/complete", GetCompleteTodos);
            group.MapGet("/notcomplete", GetNotCompleteTodos);
            group.MapGet("/priority/{todoPriority}", GetPriorityTodos);
            group.MapGet("/{id}", GetTodo);
            group.MapPost("/", CreateTodo);
            group.MapPut("/{id}", UpdateTodo);
            group.MapPatch("/{id}", PatchTodo);
            group.MapDelete("/{id}", DeleteTodo);
        }

        static async Task<Ok<List<TodoItemDto>>> GetAllTodos(ITodoService service)
        {
            return TypedResults.Ok(await service.GetAllAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetCompleteTodos(ITodoService service)
        {
            return TypedResults.Ok(await service.GetCompletedAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetNotCompleteTodos(ITodoService service)
        {
            return TypedResults.Ok(await service.GetNotCompletedAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetPriorityTodos(Todo.Priority todoPriority, ITodoService service)
        {
            return TypedResults.Ok(await service.GetPriorityAsync(todoPriority));
        }

        static async Task<Results<Ok<TodoItemDto>,NotFound>> GetTodo(int id, ITodoService service)
        {
            return await service.GetTodoByIdAsync(id)
                is TodoItemDto todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound();
        }

        static async Task<Results<Created<TodoItemDto>,ValidationProblem>> CreateTodo(TodoItemDto todo, ITodoService service)
        {
            var errors = ValidationHelper.Validate(todo);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var savedTodo = await service.CreateTodoAsync(todo);

            return TypedResults.Created($"/todoitems/{savedTodo.Id}", savedTodo);
        }

        static async Task<Results<Created<TodoItemDto>,NotFound,ValidationProblem>> UpdateTodo(int id, TodoItemDto inputTodo, ITodoService service)
        {
            var errors = ValidationHelper.Validate(inputTodo);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var todo = await service.UpdateTodoAsync(id,inputTodo);

            if (todo is null) return TypedResults.NotFound();
            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        static async Task<Results<NoContent,NotFound>> DeleteTodo(int id, ITodoService service)
        {
            bool result = await service.DeleteTodoAsync(id);
            if (result) return TypedResults.NoContent();
            return TypedResults.NotFound();
        }

        static async Task<Results<Created<TodoItemDto>, NotFound, ValidationProblem>> PatchTodo(int id, TodoPatchDto inputTodo, ITodoService service)
        {
            var errors = ValidationHelper.Validate(inputTodo);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var todo = await service.PatchTodoAsync(id,inputTodo);

            if (todo is null) return TypedResults.NotFound();

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }
    }
}
