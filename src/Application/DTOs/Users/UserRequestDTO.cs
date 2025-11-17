using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs.Users
{
 public record UserRequestDTO(
     [Required]int Id,
    [Required] string FirstName,
     [Required]string LastName,
     [Required]string Password,
    [Required , EmailAddress] string Email,
   [Required] string Phone,
   [Required] UserRole Role,
    [Required] DateTime CreatedAt,
     [Required]bool IsActive,
    string? EmployeeId,
    string? Department
);
}