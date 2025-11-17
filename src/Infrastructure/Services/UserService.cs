using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services.Auth;
using Application.DTOs.Users;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
     
     public async Task<IEnumerable<User?>> GetAllUserAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (await _userRepository.EmailExistsAsync(user.Email))
            return false;

        user.PassworedHash = _passwordService.HashPassword(user.PassworedHash);
        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        if (existingUser == null) return false;

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Phone = user.Phone;
        existingUser.UpdateAt = DateTime.Now;

        await _userRepository.UpdateAsync(existingUser);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        return true;
    }

}