namespace FMS.API.Models;

public class UserSignupDto
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
