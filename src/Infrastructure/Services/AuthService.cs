using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services.Auth;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
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
}