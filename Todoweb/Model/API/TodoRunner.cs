using Microsoft.EntityFrameworkCore;
using Todoweb.Model.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/notcomplete", GetNotCompleteTodos);
todoItems.MapGet("/priority/{todoPriority}", GetPriorityTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapPatch("/{id}", PatchTodo);
todoItems.MapDelete("/", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDto(x)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
}

static async Task<IResult> GetNotCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => !t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
}

static async Task<IResult> GetPriorityTodos(Todo.Priority todoPriority, TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.TodoPriority == todoPriority).Select(x => new TodoItemDto(x)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
            ? TypedResults.Ok(new TodoItemDto(todo))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    TodoItemDto tododto = new TodoItemDto(todo);

    return TypedResults.Created($"/todoitems/{todo.Id}", tododto);
}

static async Task<IResult> UpdateTodo(int id, TodoItemDto inputTodo, TodoDb db)
{
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

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task<IResult> PatchTodo(int id, TodoPatchDto inputTodo,  TodoDb db)
{
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