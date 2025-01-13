
using System.ComponentModel.DataAnnotations;
using server.Utils.Validations;

namespace server.DTO.User;

public class SignUpDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide valid email address.")]
    [EmailValidator(["admin", "root", "superviser", "example", "company"])]
    public required string Email { get; set; }
}
