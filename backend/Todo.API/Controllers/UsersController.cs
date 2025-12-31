using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.ExternalServices;

namespace Todo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserSyncService _ingestionService;

    public UsersController(ApplicationDbContext context, UserSyncService ingestionService)
    {
        _context = context;
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
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
}