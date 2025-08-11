using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Engine.Presentation.Filters;

public class PermissionAuthorizationAttribute(string permission) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool hasPermission = 
            user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == permission);
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
