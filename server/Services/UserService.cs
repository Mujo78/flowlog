using server.DTO.User;
using server.Services.IService;

namespace server.Services;

public class UserService(IEmailService emailService) : IUserService
{
    private readonly IEmailService emailService = emailService;

    public async Task UserSignup(SignUpDTO signUpDTO)
    {
        var token = Guid.NewGuid().ToString();
        await emailService.SendVerificationEmailAsync(signUpDTO.Email, token);
    }

    public Task<string> GenerateEmailVerificationTokenAsync()
    {
        throw new NotImplementedException();
    }
}
