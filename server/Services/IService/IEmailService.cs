using MimeKit;
using server.Utils.Email;

namespace server.Services.IService;

public interface IEmailService
{
    Task SendEmailAsync(MailData mailData, BodyBuilder bodyBuilder);
    Task SendVerificationEmailAsync(string email, string token);
    Task SendForgotPasswordEmailAsync(string email, string username, string token);
}
