using Todo.Application.Services;
using Todo.Domain.Entities;
using Xunit;

namespace Todo.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateTask_ShouldFail_InRedPhase()
    {
        // Arrange
        // Nota: Aquí el servicio pedirá un DbContext en el futuro, 
        // pero por ahora lo dejamos simple para ver el Rojo.
        var service = new TaskService(); 
        var newTask = new TaskEntity { Title = "Test" };

        // Act & Assert
        // Esperamos que falle con la excepción que escribimos
        await Assert.ThrowsAsync<NotImplementedException>(() => 
            service.CreateTaskAsync(newTask));
    }
}