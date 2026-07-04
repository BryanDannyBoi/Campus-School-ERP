using Microsoft.AspNetCore.Http;
using SchoolERP.Application.Interfaces;
using System.Security.Claims;

namespace SchoolERP.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub");
            return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : null;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}

public class FileService : IFileService
{
    private readonly string _uploadRoot;

    public FileService()
    {
        _uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(_uploadRoot);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken ct = default)
    {
        var uploadFolder = Path.Combine(_uploadRoot, folder);
        Directory.CreateDirectory(uploadFolder);

        var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var filePath = Path.Combine(uploadFolder, uniqueName);

        using var fileStreamOut = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamOut, ct);

        return Path.Combine(folder, uniqueName).Replace("\\", "/");
    }

    public Task DeleteFileAsync(string filePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_uploadRoot, filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetFileUrl(string filePath) => $"/uploads/{filePath}";
}
