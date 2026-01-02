using Microsoft.EntityFrameworkCore;
using Todo.Application.Services;
using Todo.Domain.Entities;
using Todo.Infrastructure.Data;
using Xunit;

namespace Todo.Tests;

public class TaskServiceTests
{
    private ApplicationDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new ApplicationDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task CreateTask_ShouldSaveToDatabase()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new TaskService(context);
        var newTask = new TaskEntity { Title = "Nueva Tarea", UserId = 1, PriorityId = 1 };

        // Act
        var result = await service.CreateTaskAsync(newTask);

        // Assert
        Assert.True(result.Id > 0);
        Assert.Equal("Nueva Tarea", context.Tasks.First().Title);
    }

    [Fact]
    public async Task DeleteTask_ShouldReturnTrue_WhenTaskExists()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new TaskService(context);
        var task = new TaskEntity { Id = 10, Title = "Eliminar", UserId = 1, PriorityId = 1 };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteTaskAsync(10);

        // Assert
        Assert.True(result);
        Assert.True(context.Tasks.First(t => t.Id == 10).Deleted);
    }
}