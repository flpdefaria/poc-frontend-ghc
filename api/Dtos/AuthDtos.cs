namespace Api.Dtos;

public record RegisterRequest(string? Name, string? Email, string? Password);

public record LoginRequest(string? Email, string? Password, bool Remember = false);

public record ForgotPasswordRequest(string? Email);

/// <summary>
/// Public-safe user shape returned by Register/Login. Never includes the password hash.
/// </summary>
public record AuthUserResponse(int Id, string Name, string Email);

public record MessageResponse(string Message);
