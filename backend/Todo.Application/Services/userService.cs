using Microsoft.EntityFrameworkCore;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserEntity?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserEntity> CreateUserAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}