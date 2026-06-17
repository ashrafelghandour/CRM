using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
namespace WebAPI.Controllers.Auth;
public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService , IUserService userService)
    {
        _authService = authService;
        _userService = userService; 
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
            var refreshToken =  GenerateRefreshToken();
            var response = new LoginResponse(token,
                $"{user.FirstName} {user.LastName}",
                user.Email,
               user.Role.ToString(),
              DateTime.Now.AddMinutes(60),
                refreshToken
              
            
            );
             user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
                user.RefreshTokenExpiryAt = DateTime.UtcNow.AddDays(7);
                user.RefreshTokenRevokedAt = null;
                await _userService.UpdateUserAsync(user);
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
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequst request)
    {
        try
        {
            var response = await _authService.GenerateRefreshTokenAsync(request.Email, request.RefreshToken);
            if (string.IsNullOrEmpty(response) )
                return HandleError("Invalid refresh token", 401);

            return Ok(new { 
                success = true, 
                message = "Token refreshed successfully", 
                data = response 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Token refresh failed: {ex.Message}");
        }
    }
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LogoutRequst logoutRequest)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(logoutRequest.Email);
            if (user == null)
                return Ok();
             
            bool isValidRefreshToken = BCrypt.Net.BCrypt.Verify(logoutRequest.RefreshToken, user.RefreshTokenHash);
            if (!isValidRefreshToken)
                return Ok();
        
            
            user.RefreshTokenRevokedAt = DateTime.UtcNow;
            await _userService.UpdateUserAsync(user);


            return Ok("Logout successful");
        }
        catch (Exception ex)
        {
            return Ok();
        }
    }
    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    
}