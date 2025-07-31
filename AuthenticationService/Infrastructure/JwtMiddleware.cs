using AuthenticationService.Models;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Infrastructure
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _settings;
        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) 
            {
                var status = TokenManager.ValidateToken(token, _settings, context); 

                // Token is valid, user information is set in context

            }
            await _next(context);
        }
    }
   
}
