using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services.Auth;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService( IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService, IConfiguration configuration  )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive) return null;

        if (_passwordService.VerifyPassword(password, user.PassworedHash))
        {
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return user;
        }

        return null;
    }

    public async Task<bool> ChangePassworedAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        if (!_passwordService.VerifyPassword(currentPassword, user.PassworedHash))
            return false;

        user.PassworedHash = _passwordService.HashPassword(newPassword);
        user.UpdateAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        return true;
    }

   

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        return _tokenService.GeneratToken(user);
    }
    public async Task<string> GenerateRefreshTokenAsync(string email, string refreshToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return string.Empty;

        if (user.RefreshTokenRevokedAt != null || user.RefreshTokenExpiryAt <= DateTime.UtcNow)
            return string.Empty;

      
            if (!BCrypt.Net.BCrypt.Verify(refreshToken, user.RefreshTokenHash))
                return string.Empty;

             var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
                user.RefreshTokenExpiryAt = DateTime.UtcNow.AddDays(7);
                user.RefreshTokenRevokedAt = null;
                await _userRepository.UpdateAsync(user);
               
                return newRefreshToken;
              
                
        
}

 private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}