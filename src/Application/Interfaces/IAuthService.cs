using System.Runtime.ExceptionServices;
using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string email,string passwored);
        Task<bool> ChangePassworedAsync(int userid,string CurrentPasswored,string newPasswored);
        Task<string> GenerateJwtTokenAsync(User user);
    
    }
    public interface ITokenService
    {
        string GeneratToken(User user);
    }
}