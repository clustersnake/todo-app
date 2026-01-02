using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.ExternalServices;
using Todo.Application.Interfaces;
using Todo.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtener la cadena de conexión de appsettings.Development.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registrar el DbContext con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<UserSyncService>();

builder.Services.AddControllers();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
    
}
app.UseMiddleware<Todo.API.Middleware.ApiKeyMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.MapControllers();

app.Run();


