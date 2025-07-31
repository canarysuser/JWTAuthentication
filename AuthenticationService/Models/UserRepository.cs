namespace AuthenticationService.Models
{
    public class UserRepository
    {
        UsersDbContext _context;
        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {

            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password)!;
        }
        public Role GetRoleByUserId(int userId)
        {
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            if (userRole == null) return null;
            return _context.Roles.FirstOrDefault(r => r.RoleId == userRole.RoleId)!;
        }
    }
}
