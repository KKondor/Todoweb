using Microsoft.EntityFrameworkCore;
using Todoweb;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/notcomplete", async (TodoDb db) =>
    await db.Todos.Where(t => !t.IsComplete).ToListAsync());

app.MapGet("/todoitems/lowpriority", async (TodoDb db) =>
    await db.Todos.Where(t => t.TodoPriority == Todo.Priority.Low_Priority).ToListAsync());

app.MapGet("/todoitems/normalpriority", async (TodoDb db) =>
    await db.Todos.Where(t => t.TodoPriority == Todo.Priority.Normal_Priority).ToListAsync());

app.MapGet("/todoitems/urgentpriority", async (TodoDb db) =>
    await db.Todos.Where(t => t.TodoPriority == Todo.Priority.Urgent_Priority).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapPatch("/todoitems/{id}", async (int id, TodoPatchDto inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    if (inputTodo.Name is not null) todo.Name = inputTodo.Name;
    if (inputTodo.IsComplete is not null) todo.IsComplete = inputTodo.IsComplete.Value;
    if (inputTodo.Description is not null) todo.Description = inputTodo.Description;
    if (inputTodo.TodoPriority is not null) todo.TodoPriority = inputTodo.TodoPriority.Value;
    if (inputTodo.DueDate is not null) todo.DueDate = inputTodo.DueDate;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();