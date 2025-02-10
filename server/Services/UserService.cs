using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO.User;
using server.Models;
using server.Repository.IRepository;
using server.Services.IService;

namespace server.Services;

public class UserService(IEmailService emailService, IUserRepository repository, UserManager<ApplicationUser> userManager, ApplicationDBContext db) : IUserService
{
    private readonly IEmailService emailService = emailService;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IUserRepository userRepository = repository;
    private readonly ApplicationDBContext db = db;

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
        if (userRepository.EmailAlreadyUsed(email))
        {
            throw new Exception("Email already used.");
        }

        var emailToken = await userRepository.GetEmailTokenByEmail(email) ?? throw new Exception("Email not found.");
        var roleId = await userRepository.GetRoleByName("User") ?? throw new Exception("Role not found.");

        var user = new ApplicationUser
        {
            Email = email,
            UserName = registrationDTO.Username,
            RoleId = roleId,
            EmailConfirmed = true
        };

        var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            await userRepository.DeleteUserEmailToken(emailToken);
            await userManager.CreateAsync(user, registrationDTO.Password);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
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

    public async Task ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email) ?? throw new Exception("User not found.");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        try
        {
            await emailService.SendForgotPasswordEmailAsync(forgotPasswordDTO.Email, user.UserName!, token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email) ?? throw new Exception("User not found.");

        await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPasswordDTO.Token);
        throw new NotImplementedException();
    }
}
