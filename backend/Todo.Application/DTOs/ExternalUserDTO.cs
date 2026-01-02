namespace Todo.Application.DTOs;

public class ExternalUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "Leanne Graham"
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}