using System.Text.RegularExpressions;
using Api.Data;
using Api.Dtos;
using Api.Models;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    private static readonly Regex EmailPattern = new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);
    private const string InvalidCredentialsMessage = "Invalid email or password.";
    private const string ForgotPasswordSuccessMessage =
        "If an account exists for this email, you'll receive a reset link shortly.";

    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/forgot-password", ForgotPassword);
    }

    private static async Task<IResult> Register(RegisterRequest request, AppDbContext db)
    {
        var name = request.Name?.Trim() ?? "";
        var email = request.Email?.Trim() ?? "";
        var password = request.Password ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            return Results.BadRequest(new MessageResponse("Full name is required."));
        }

        if (string.IsNullOrWhiteSpace(email) || !EmailPattern.IsMatch(email))
        {
            return Results.BadRequest(new MessageResponse("Enter a valid email address."));
        }

        if (password.Length < 8)
        {
            return Results.BadRequest(new MessageResponse("Password must be at least 8 characters."));
        }

        var emailExists = await db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        if (emailExists)
        {
            return Results.Conflict(new MessageResponse("An account with this email already exists."));
        }

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = PasswordHasher.Hash(password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Results.Created($"/api/users/{user.Id}", new AuthUserResponse(user.Id, user.Name, user.Email));
    }

    private static async Task<IResult> Login(LoginRequest request, AppDbContext db)
    {
        var email = request.Email?.Trim() ?? "";
        var password = request.Password ?? "";

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        // Same generic error for "no such user" and "wrong password" — avoids account enumeration.
        if (user is null || !PasswordHasher.Verify(password, user.PasswordHash))
        {
            return Results.Json(new MessageResponse(InvalidCredentialsMessage), statusCode: StatusCodes.Status401Unauthorized);
        }

        return Results.Ok(new AuthUserResponse(user.Id, user.Name, user.Email));
    }

    private static IResult ForgotPassword(ForgotPasswordRequest request)
    {
        var email = request.Email?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(email) || !EmailPattern.IsMatch(email))
        {
            return Results.BadRequest(new MessageResponse("Enter a valid email address."));
        }

        // Always return the same generic success message, regardless of whether the account
        // exists, to avoid leaking which emails are registered (OWASP A07).
        return Results.Ok(new MessageResponse(ForgotPasswordSuccessMessage));
    }
}
