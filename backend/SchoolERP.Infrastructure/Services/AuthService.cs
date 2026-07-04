using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.Interfaces;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Persistence.Context;

namespace SchoolERP.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly SchoolERPDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public AuthService(SchoolERPDbContext context, ITokenService tokenService, IPasswordService passwordService)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted, ct)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account has been deactivated.");

        if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
            User = MapUser(user)
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token has expired.");

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync(ct);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
            User = MapUser(user)
        };
    }

    public async Task LogoutAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _context.Users.FindAsync(new object[] { userId }, ct);
        if (user is not null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ForgotPasswordAsync(string email, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null) return false;

        user.PasswordResetToken = Guid.NewGuid().ToString("N");
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        await _context.SaveChangesAsync(ct);
        // Email sending would be triggered here
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == dto.Email && u.PasswordResetToken == dto.Token, ct);

        if (user is null || user.PasswordResetTokenExpiry < DateTime.UtcNow) return false;

        user.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default)
    {
        var user = await _context.Users.FindAsync(new object[] { userId }, ct);
        if (user is null) return false;

        if (!_passwordService.VerifyPassword(dto.CurrentPassword, user.PasswordHash)) return false;

        user.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    private static UserDto MapUser(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role.ToString(),
        ProfilePhoto = user.ProfilePhoto,
        PhoneNumber = user.PhoneNumber,
        IsActive = user.IsActive
    };
}
