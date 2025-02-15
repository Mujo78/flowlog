using System.ComponentModel.DataAnnotations;
using server.Utils.Validations;

namespace server.DTO.Auth;

public class LoginDTO
{
    [Required(ErrorMessage = "Email or username is required.")]
    [EmailOrUsernameValidation]
    public string EmailOrUsername { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
