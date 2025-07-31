using AuthenticationService.Infrastructure;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserRepository _repository;
        public UsersController(UserRepository repository)
        {
            _repository = repository;
        }

        [CustomAuthorization(AllowedRole = "Test")]
        [HttpGet("role/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var user = _repository.GetRoleByUserId(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }
    }
}
