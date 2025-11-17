using System.Net;
using Application.DTOs.Users;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers;

namespace Presintation.Controllers.Users;
    [Authorize]
    [Route("[controller]")]
    public class UserController :ApiControllerBase
    {
       private readonly IUserService _userService;
       public UserController(IUserService userService)
       {
        _userService = userService;
       }
       [HttpGet]     

       public async Task<IActionResult> GetAllUsers()
        { 
                try{
                if(GetUserRole()!=UserRole.SuperAdmin.ToString())
                    return HandleError("Access denied",4003);

                    var users = await _userService.GetAllUserAsync();
                    
                    if(users is null || users.Any()==false)
                    return HandleError("Not have any users",4004);
                    
                    IEnumerable<UserResponseDTO> dTO = users.Select( u => new UserResponseDTO(
                        u.Id,u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Phone,
                        u.Role,
                        u.CreatedAt,
                        u.IsActive,
                        null,null));
                    return Ok(new {
                        success =true ,
                        data = dTO
                    });
            }
            catch(Exception ex)
            {
                return HandleError($"Failed to retrieve users: {ex.Message}");
            }
        }
        
    
     
       [HttpGet("{id}")]
       public async Task<IActionResult> GetUserById(int id)
        { 
                try{
                    var currentUserId = GetUserId();
                    var currentUserRole = GetUserRole();

                     // Users can only view their own profile unless they're SuperAdmin
                      if (currentUserId != id && currentUserRole != UserRole.SuperAdmin.ToString())
                       return HandleError("Access denied", 403);

                        var u = await _userService.GetUserByIdAsync(id);
                        
                        if(u is null )
                        return HandleError("User not found",4004);
                        
                        UserResponseDTO dTO = new UserResponseDTO(
                            u.Id,
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.Phone,
                            u.Role,
                            u.CreatedAt,
                            u.IsActive,
                            null,null);

                        return HandleResult(dTO,"User retrieved successfully");
            }
            catch(Exception ex)
            {
                return HandleError($"Failed to retrieve users: {ex.Message}");
            }
        }
          
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO request)
    {
        try
        {
            // Only SuperAdmin can create users
            if (GetUserRole() != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            User user;

            if (request.Role == UserRole.Employee)
            {
                user = new SystemUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Role = request.Role,
                    PassworedHash =request.Password, // Will be hashed in service
                    EmployeeId = request.EmployeeId ?? string.Empty,
                    Department = request.Department ?? string.Empty,
                    CreatedAt = DateTime.Now
                };
            }
            else
            {
                user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Role = request.Role,
                    PassworedHash = request.Password // Will be hashed in service
                };
            }

            var result = await _userService.CreateUserAsync(user);
            if (!result)
                return HandleError("Email already exists");

            var userResponse = new UserResponseDTO(
                 user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Phone,
                user.Role,
                user.CreatedAt,
                user.IsActive,null,null);
            

            return CreatedAtAction(nameof(GetUserById),new {id = user.Id},
            new {success = true, 
                message = "User created successfully", 
                data = userResponse });

        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create user: {ex.Message}");
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserResponseDTO request)
    {
        try
        {
            var currentUserId = GetUserId();
            var currentUserRole = GetUserRole();

            // Users can only update their own profile unless they're SuperAdmin
            if (currentUserId != id && currentUserRole != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return HandleError("User not found", 404);

            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.Phone = request.Phone;

            var result = await _userService.UpdateUserAsync(existingUser);
            if (!result)
                return HandleError("Failed to update user");

            return Ok(new { 
                success = true, 
                message = "User updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update user: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            // Only SuperAdmin can delete users
            if (GetUserRole() != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return HandleError("User not found", 404);

            return Ok(new { 
                success = true, 
                message = "User deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete user: {ex.Message}");
        }
    }
}
