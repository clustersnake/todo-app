using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
using Todo.Infrastructure.ExternalServices;

namespace Todo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly UserSyncService _ingestionService;

    public UsersController(IUserService userService, UserSyncService ingestionService)
    {
        // _context = context;
        _userService = userService;
        _ingestionService = ingestionService;
    }

    /// <summary>
    /// Obtiene y almacena usuarios desde una API externa
    /// </summary>
    /// <returns></returns>
    [HttpPost("sync")]
    public async Task<IActionResult> Sync()
    {
        await _ingestionService.SyncUsersAsync();
        return Ok(new { message = "Ingesta completada" });
    }

    /// <summary>
    /// Mostrar todos los usuarios almacenados
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: mydomain.com/api/users/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user != null ? Ok(user) : NotFound();
    }

    // POST: mydomain.com/api/users/user
    [HttpPost("user")]
    public async Task<IActionResult> Create(UserEntity user)
    {
        var created = await _userService.CreateUserAsync(user);
        return Ok(created);
    }
}