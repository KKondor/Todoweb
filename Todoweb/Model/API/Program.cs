using Microsoft.EntityFrameworkCore;
using Todoweb.Model.API;
using Todoweb.Model.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGroup("/todoitems").WithTags("TodoModel").MapTodoEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();