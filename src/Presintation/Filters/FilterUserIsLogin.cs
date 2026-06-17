
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presintation.Filters;

public class FilterUserIsLoginAttribute(IUserService _userService) :    ActionFilterAttribute
{ 
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userRefreshTokenRevokedAtClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RefreshTokenRevokedAt");
        if(!string.IsNullOrEmpty(userRefreshTokenRevokedAtClaim?.Value) )
        {
            
                
                
                    context.HttpContext.Response.StatusCode = 401; // Unauthorized
                    context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { success = false, message = "Session expired. Please log in again." });
            return;
            
        }
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
      throw new NotImplementedException();
    }


 
}
