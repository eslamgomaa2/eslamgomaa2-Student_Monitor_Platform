using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Teacher"; // Teacher, Student, Admin
    public string Password { get; set; } = string.Empty;
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool AgreeToTerms { get; set; }
}