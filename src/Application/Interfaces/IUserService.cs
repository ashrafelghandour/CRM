using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Users;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User?>GetUserByIdAsync(int id);
        Task<IEnumerable<User?>>GetAllUserAsync();
         Task<User?>GetUserByEmailAsync(string Email);
         Task<bool>CreateUserAsync(User user);
         Task<bool>UpdateUserAsync(User user);
         Task<bool>DeleteUserAsync(int userId);


    }
}