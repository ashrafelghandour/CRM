using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public record LoginResponse(
       [Required] string Token,
       [Required] string FullName ,
       [Required , EmailAddress] string Email,
       [Required] string Role,
       [Required] DateTime Expiration 
       );

       }