using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Todoweb.Model.API;
using Todoweb.Model.Helpers;

namespace Todoweb.Model.Endpoints
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
            group.MapDelete("/", DeleteTodo);
        }

        static async Task<Ok<List<TodoItemDto>>> GetAllTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDto(x)).ToListAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetCompleteTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetNotCompleteTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => !t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
        }

        static async Task<Ok<List<TodoItemDto>>> GetPriorityTodos(Todo.Priority todoPriority, TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => t.TodoPriority == todoPriority).Select(x => new TodoItemDto(x)).ToListAsync());
        }

        static async Task<Results<Ok<TodoItemDto>,NotFound>> GetTodo(int id, TodoDb db)
        {
            return await db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(new TodoItemDto(todo))
                    : TypedResults.NotFound();
        }

        static async Task<Results<Created<TodoItemDto>,BadRequest<List<ValidationResult>>>> CreateTodo(Todo todo, TodoDb db)
        {
            var errors = ValidationHelper.Validate(todo);
            if (errors.Count > 0)
                return TypedResults.BadRequest(errors);

            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            TodoItemDto tododto = new TodoItemDto(todo);

            return TypedResults.Created($"/todoitems/{todo.Id}", tododto);
        }

        static async Task<Results<NoContent,NotFound,BadRequest<List<ValidationResult>>>> UpdateTodo(int id, TodoItemDto inputTodo, TodoDb db)
        {
            var errors = ValidationHelper.Validate(inputTodo);
            if (errors.Count > 0)
                return TypedResults.BadRequest(errors);

            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;
            todo.TodoPriority = inputTodo.TodoPriority;
            todo.Description = inputTodo.Description;
            todo.DueDate = inputTodo.DueDate;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent,NotFound>> DeleteTodo(int id, TodoDb db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<List<ValidationResult>>>> PatchTodo(int id, TodoPatchDto inputTodo, TodoDb db)
        {
            var errors = ValidationHelper.Validate(inputTodo);
            if (errors.Count > 0)
                return TypedResults.BadRequest(errors);

            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            if (inputTodo.Name is not null) todo.Name = inputTodo.Name;
            if (inputTodo.IsComplete is not null) todo.IsComplete = inputTodo.IsComplete.Value;
            if (inputTodo.Description is not null) todo.Description = inputTodo.Description;
            if (inputTodo.TodoPriority is not null) todo.TodoPriority = inputTodo.TodoPriority.Value;
            if (inputTodo.DueDate is not null) todo.DueDate = inputTodo.DueDate;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}
