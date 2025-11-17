using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    protected string GetUserEmail()
    {
        var emailClaim = User.FindFirst(ClaimTypes.Email);
        return emailClaim?.Value ?? string.Empty;
    }

    protected string GetUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? string.Empty;
    }

    protected IActionResult HandleResult<T>(T result, string successMessage = "Operation completed successfully")
    {
        if (result == null)
            return NotFound(new { message = "Resource not found" });

        return Ok(new { 
            success = true, 
            message = successMessage, 
            data = result 
        });
    }

    protected IActionResult HandleError(string errorMessage, int statusCode = 400)
    {
        return statusCode switch
        {
            400 => BadRequest(new { success = false, message = errorMessage }),
            404 => NotFound(new { success = false, message = errorMessage }),
            401 => Unauthorized(new { success = false, message = errorMessage }),
            _ => BadRequest(new { success = false, message = errorMessage })
        };
    }
}