using Todo.Domain.Entities;

namespace Todo.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(int userId);
    Task<UserEntity> CreateUserAsync(UserEntity user);
}