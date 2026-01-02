using Todo.Application.Interfaces;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class TaskService : ITaskService
{
    public Task<TaskEntity> CreateTaskAsync(TaskEntity task)
    {
        // El test fallará aquí con esta excepción
        throw new NotImplementedException("Fase Rojo: El método de creación no está implementado.");
    }

    public Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId)
    {
        throw new NotImplementedException("Fase Rojo: El método de consulta no está implementado.");
    }

    public Task<bool> UpdateTaskAsync(int idTask, TaskEntity task)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTaskAsync(int idTask)
    {
        throw new NotImplementedException();
    }
}