using Microsoft.EntityFrameworkCore;
using Todoweb.Backend.Model.API;
using Todoweb.Backend.Model.Endpoints;
using Todoweb.Backend.Repositories;
using Todoweb.Backend.Service;

var MyAllowedSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: MyAllowedSpecificOrigins, policy =>
    {
        policy.WithOrigins(builder.Configuration.GetConnectionString("DefaultFrontEnd")).AllowAnyHeader().AllowAnyMethod();
    });
}
);

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDB")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));
app.UseStatusCodePages(statusCodeHandlerApp =>
{
    statusCodeHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null
            || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        }
    });
});
app.UseCors(MyAllowedSpecificOrigins);
app.MapGroup("/todoitems").WithTags("TodoModel").MapTodoEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();