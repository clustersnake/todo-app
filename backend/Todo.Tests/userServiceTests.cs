using Microsoft.EntityFrameworkCore;
using Todo.Application.Services;
using Todo.Domain.Entities;
using Todo.Infrastructure.Data;
using Xunit;

namespace Todo.Tests;

public class UserServiceTests
{
    private ApplicationDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task CreateUser_ShouldSaveCorrectly()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new UserService(context);
        var newUser = new UserEntity { FirstName = "Test", LastName = "User", Email = "test@example.com", Phone = "123" };

        // Act
        var result = await service.CreateUserAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", context.Users.First().FirstName);
    }
}