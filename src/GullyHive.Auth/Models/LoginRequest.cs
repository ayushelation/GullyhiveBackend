namespace GullyHive.Auth.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!; // email OR phone
        public string Password { get; set; } = null!;
    }

}