using Microsoft.EntityFrameworkCore;
using Todoweb.Backend.Model.API;
using Todoweb.Backend.Model.Endpoints;
using Todoweb.Backend.Repositories;
using Todoweb.Backend.Service;

var MyAllowedSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: MyAllowedSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
}
);

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.UseCors(MyAllowedSpecificOrigins);
app.MapGroup("/todoitems").WithTags("TodoModel").MapTodoEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();