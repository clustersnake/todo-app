using System.Net.Http.Json;
using Todo.Application.DTOs;
using Todo.Domain.Entities;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.ExternalServices;

public class UserSyncService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;

    public UserSyncService(HttpClient httpClient, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task SyncUsersAsync()
    {
        // 1. Consumir la API externa
        var externalUsers = await _httpClient.GetFromJsonAsync<List<ExternalUserDto>>(
            "https://jsonplaceholder.typicode.com/users");

        if (externalUsers != null)
        {
            foreach (var user in externalUsers)
            {
                // Evitar duplicados por Email
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    // 2. Mapear DTO a Entity
                    var names = user.Name.Split(' ');
                    var newUser = new UserEntity
                    {
                        FirstName = names[0],
                        LastName = names.Length > 1 ? names[1] : "N/A",
                        Email = user.Email,
                        Phone = user.Phone,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(newUser);
                }
            }
            // 3. Guardar en Supabase
            await _context.SaveChangesAsync();
        }
    }
}