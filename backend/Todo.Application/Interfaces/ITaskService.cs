using Todo.Domain.Entities;

namespace Todo.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId);
    Task<TaskEntity> CreateTaskAsync(TaskEntity task);
    Task<bool> UpdateTaskAsync(int idTask, TaskEntity task);
    Task<bool> DeleteTaskAsync(int idTask);
}