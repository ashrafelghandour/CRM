using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public record LogoutRequst(
         [Required ,EmailAddress] string Email,
        [Required] string RefreshToken
    );
   
}