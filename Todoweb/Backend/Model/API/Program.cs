using Microsoft.EntityFrameworkCore;
using Todoweb.Backend.Model.API;
using Todoweb.Backend.Model.Endpoints;
using Todoweb.Backend.Repositories;
using Todoweb.Backend.Service;

var MyAllowedSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
Args = args,
EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.Sources.Clear();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);

builder.Host.ConfigureAppConfiguration(config =>
{
    foreach (var source in config.Sources)
    {
        if (source is Microsoft.Extensions.Configuration.FileConfigurationSource fileSource)
            fileSource.ReloadOnChange = false;
    }
});
builder.Services.AddProblemDetails();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(MyAllowedSpecificOrigins, policy =>
    {
        policy.WithOrigins(builder.Configuration["DefaultFrontEnd"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultDB")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDb>();
    db.Database.Migrate();
}
app.UseCors(MyAllowedSpecificOrigins);
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
app.MapGroup("/todoitems").WithTags("TodoModel").MapTodoEndpoints();
app.Run();