using Microsoft.EntityFrameworkCore;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;


namespace Todo.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PriorityEntity> Priorities { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Configurar la relación entre TaskEntity con UserEntity y PriorityEntity 

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);

        
        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Priority)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.PriorityId);
    }
}
