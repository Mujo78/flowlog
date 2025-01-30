using System.Security.Cryptography;
using System.Text;
using server.DTO.User;
using server.Models;
using server.Repository.IRepository;
using server.Services.IService;

namespace server.Services;

public class UserService(IEmailService emailService, IUserRepository repository) : IUserService
{
    private readonly IEmailService emailService = emailService;
    private readonly IUserRepository userRepository = repository;

    public async Task UserSignup(SignUpDTO signUpDTO)
    {
        if (userRepository.IsEmailTaken(signUpDTO.Email))
        {
            throw new Exception("Email already used, please check your email inbox!");
        }

        if (userRepository.EmailAlreadyUsed(signUpDTO.Email))
        {
            throw new Exception("Email already used");
        }

        var token = Guid.NewGuid().ToString();
        string tokenToSave = GenerateEmailVerificationTokenAsync(token);

        EmailVerificationToken emailVerificationToken = new()
        {
            Email = signUpDTO.Email,
            Token = tokenToSave,
            ExpirationTime = DateTime.UtcNow.AddHours(24)
        };

        try
        {
            await userRepository.CreateUserEmailToken(emailVerificationToken);
            await emailService.SendVerificationEmailAsync(signUpDTO.Email, token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string GenerateEmailVerificationTokenAsync(string generatedToken)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(generatedToken));
        return Convert.ToBase64String(bytes);
    }
}
