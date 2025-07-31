

using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationService.Infrastructure
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public string AllowedRole { get; set; }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext.Items.TryGetValue("Role", out var role) && 
               role is string userRole && 
               userRole.Equals(AllowedRole, StringComparison.OrdinalIgnoreCase))
            {
                // User is authorized, do nothing
                return;
            } else
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult() ;
            }
        }
    }
}
