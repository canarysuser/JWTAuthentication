using AuthenticationService.Infrastructure;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserRepository _repository;
        AppSettings _settings; 
        public AuthController(UserRepository repository, IOptions<AppSettings> settings)
        {
            _repository = repository;
            _settings = settings.Value;
        }
        [HttpPost("login")]
        public IActionResult  Login(AuthenticateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid login request.");
            }
            var user = _repository.Authenticate(request.Username, request.Password);
            if(user==null) 
                return Unauthorized("Invalid username or password.");


            var role = _repository.GetRoleByUserId(user.UserId);
            if (role == null)
            {
                return NotFound("Role not found for the user.");
            }
            var token = TokenManager.GenerateToken(user, role, _settings);
            var response = new AuthenticateResponse
            {
                Token = token,
                EmailId = user.Email,
                UserId = user.UserId,
                Role = role.RoleName,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            return Ok(response);
        }
    }
}
