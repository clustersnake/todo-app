using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;

namespace Todo.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<TaskEntity> Tasks { get; }
    DbSet<PriorityEntity> Priorities { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}