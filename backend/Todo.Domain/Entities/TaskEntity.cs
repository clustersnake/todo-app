namespace Todo.Domain.Entities;

public class TaskEntity
{
    public int Id { get; set; }
    
    // Relación con Usuario
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Completed { get; set; } = false;
    public bool Deleted { get; set; } = false;
    
    // Almacenamiento de Tags como string (ej: "urgente,trabajo")
    public string Tags { get; set; } = string.Empty;

    // Relación con Prioridad
    public int PriorityId { get; set; }
    public virtual PriorityEntity Priority { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}