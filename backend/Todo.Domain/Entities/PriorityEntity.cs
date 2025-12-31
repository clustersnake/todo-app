namespace Todo.Domain.Entities;

public class PriorityEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Propiedad de navegación: Una prioridad puede estar en muchas tareas
    public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}