using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.Auth;
public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _authService.AuthenticateAsync(request.Email,request.Passwored);
            if (user == null)
                return HandleError("Invalid email or password", 401);

            var token = await _authService.GenerateJwtTokenAsync(user);

            var response = new LoginResponse(token,
                $"{user.FirstName} {user.LastName}",
                user.Email,
               user.Role.ToString(),
              DateTime.Now.AddMinutes(60)
            );

            return Ok(new { 
                success = true, 
                message = "Login successful", 
                data = response 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Login failed: {ex.Message}");
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {

        try
        {
            var userId = GetUserId();
            if (userId == 0)
                return HandleError("User not authenticated", 401);

            if (request.NewPassword != request.ConfirmPassword)
                return HandleError("New password and confirmation don't match");

            var result = await _authService.ChangePassworedAsync(userId, request.CurrentPassword, request.NewPassword);
            
            if (!result)
                return HandleError("Current password is incorrect");

            return Ok(new { 
                success = true, 
                message = "Password changed successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Password change failed: {ex.Message}");
        }
    }
}