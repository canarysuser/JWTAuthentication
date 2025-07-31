namespace AuthenticationService.Models
{
    //Section would be generated/specified in the appsettings.json file 
    public class AppSettings
    {
        public string SecretKey { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456";
        public string AppName { get; set; }
        public int AppId { get; set; }
    }

    //Request Model 
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    //Response Model 
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public string EmailId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public DateTime Expires { get; set; }
    }
}
