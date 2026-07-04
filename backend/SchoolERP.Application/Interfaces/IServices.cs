using SchoolERP.Application.DTOs.Auth;

namespace SchoolERP.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task LogoutAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ForgotPasswordAsync(string email, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default);
}

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, string email, string role);
    string GenerateRefreshToken();
    Guid? ValidateToken(string token);
}

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}

public interface IFileService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken ct = default);
    Task DeleteFileAsync(string filePath, CancellationToken ct = default);
    string GetFileUrl(string filePath);
}
