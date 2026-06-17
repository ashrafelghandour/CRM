using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public record RefreshRequst(
        [Required ,EmailAddress] string Email,
        [Required] string RefreshToken
    ); 

       
   
}