using Microsoft.AspNetCore.Mvc;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;

namespace Todo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUser(int userId) => 
        Ok(await _taskService.GetTasksByUserIdAsync(userId));

    [HttpPost]
    public async Task<IActionResult> Create(TaskEntity task) => 
        Ok(await _taskService.CreateTaskAsync(task));

    [HttpPut("{idTask}")]
    public async Task<IActionResult> Update(int idTask, TaskEntity task) => 
        await _taskService.UpdateTaskAsync(idTask, task) ? Ok() : NotFound();

    [HttpDelete("{idTask}")]
    public async Task<IActionResult> Delete(int idTask) => 
        await _taskService.DeleteTaskAsync(idTask) ? Ok() : NotFound();
}