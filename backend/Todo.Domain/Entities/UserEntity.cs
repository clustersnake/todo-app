namespace Todo.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Propiedad de navegación: Un usuario tiene muchas tareas
    public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

}