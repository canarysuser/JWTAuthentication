using AuthenticationService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthenticationService.Infrastructure
{
    public class TokenManager
    {

        public static string GenerateToken(User user, Role role, AppSettings settings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(settings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("appId", settings.AppId.ToString()),
                    new Claim("emailId", user.Email),
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("roleName", role.RoleName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static bool ValidateToken(string token, AppSettings settings, HttpContext context)
        {
           // principal = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = System.Text.Encoding.ASCII.GetBytes(settings.SecretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Disable clock skew
                };
                
                var principal =  tokenHandler.ValidateToken(token, validationParameters, out _);
                context.Items["UserId"] = principal.FindFirst("userId")?.Value;
                context.Items["Role"] = principal.FindFirst("roleName")?.Value;
                context.Items["EmailId"] = principal.FindFirst("emailId")?.Value;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
