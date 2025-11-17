using Application.DTOs.Users;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> EmailExistsAsync(string email);
    
    }
}
