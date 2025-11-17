using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Users
{
    public record UserResponseDTO(
    
    int Id,
    
    [property: Required, StringLength(50)]
    string FirstName,
    
    [property: Required, StringLength(50)]
    string LastName,
    
    [property: Required, EmailAddress]
    string Email,
    
    [property: Phone]
    string Phone,
    
    UserRole Role,
    
    DateTime CreatedAt,
    
    bool IsActive,
    
    // For SystemUser
    string? EmployeeId,
    
    string? Department
);
}