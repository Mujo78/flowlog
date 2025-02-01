using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
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

    public async Task VerifyEmail(string token)
    {
        string hashedToken = GenerateEmailVerificationTokenAsync(token);
        var storedToken = await userRepository.GetEmailToken(hashedToken) ?? throw new Exception("Invalid token provided. Token not found or expired.");

        if (!storedToken.Verified && IsEmailTokenValid(storedToken))
        {
            storedToken.Verified = true;
            await userRepository.VerifyEmailToken(storedToken);
        }
        else
        {
            throw new Exception("Token expired or already verified.");
        }
    }

    public async Task UserRegistration(RegistrationDTO registrationDTO, string email)
    {

    }

    public string GenerateEmailVerificationTokenAsync(string generatedToken)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(generatedToken));
        return Convert.ToBase64String(bytes);
    }

    public bool IsEmailTokenValid(EmailVerificationToken token)
    {
        return token.ExpirationTime > DateTime.UtcNow;
    }
}
