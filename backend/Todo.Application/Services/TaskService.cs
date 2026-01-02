using Microsoft.EntityFrameworkCore;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class TaskService : ITaskService
{

    private readonly IApplicationDbContext _context;

    public TaskService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
    {
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId)
    {
        return await _context.Tasks
    .Where(t => t.UserId == userId && !t.Deleted)
    .ToListAsync();
    }

public async Task<bool> UpdateTaskAsync(int idTask, TaskEntity task)
{
    var existingTask = await _context.Tasks.FindAsync(idTask);
    if (existingTask == null) return false;

    existingTask.Title = task.Title;
    existingTask.Description = task.Description;
    existingTask.Completed = task.Completed;
    existingTask.PriorityId = task.PriorityId;
    existingTask.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();
    return true;
}

public async Task<bool> DeleteTaskAsync(int idTask)
{
    var task = await _context.Tasks.FindAsync(idTask);
    if (task == null) return false;

    // Soft Delete (recomendado) o Hard Delete
    task.Deleted = true; 
    task.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();
    return true;
}
}