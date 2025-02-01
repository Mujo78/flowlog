using System.ComponentModel.DataAnnotations;
using server.Utils.Validations;

namespace server.DTO.User;

public class RegistrationDTO
{
    [Required]
    [MinLength(6, ErrorMessage = "Username must be at least 6 characters long.")]
    public string Username { get; set; }
    [Required]
    [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
    [PasswordValidation("Password")]
    public string Password { get; set; }
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
