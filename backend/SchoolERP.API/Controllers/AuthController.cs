using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.Interfaces;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("logout"), Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty) return Unauthorized();
        await _authService.LogoutAsync(userId);
        return Ok(ApiResponse<object>.Ok(null, "Logged out successfully"));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        await _authService.ForgotPasswordAsync(dto.Email);
        return Ok(ApiResponse<object>.Ok(null, "If email exists, a reset link has been sent."));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var success = await _authService.ResetPasswordAsync(dto);
        return success
            ? Ok(ApiResponse<object>.Ok(null, "Password reset successfully"))
            : BadRequest(ApiResponse<object>.Fail("Invalid or expired reset token."));
    }

    [HttpPost("change-password"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = GetCurrentUserId();
        var success = await _authService.ChangePasswordAsync(userId, dto);
        return success
            ? Ok(ApiResponse<object>.Ok(null, "Password changed successfully"))
            : BadRequest(ApiResponse<object>.Fail("Current password is incorrect."));
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }
}
