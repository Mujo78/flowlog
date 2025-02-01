using server.DTO.User;

namespace server.Services.IService;

public interface IUserService
{
    Task UserSignup(SignUpDTO signUpDTO);
    Task VerifyEmail(string token);
    Task UserRegistration(RegistrationDTO registrationDTO, string email);
}
