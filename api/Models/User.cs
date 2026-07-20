namespace Api.Models;

/// <summary>
/// EF Core entity representing a mock user record (in-memory database only — data
/// resets whenever the process restarts, no persistence to disk).
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
