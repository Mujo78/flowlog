using System.ComponentModel.DataAnnotations;

namespace server.DTO.User;

public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide valid email address.")]
    public required string Email { get; set; }
}
