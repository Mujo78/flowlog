
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using server.Services.IService;
using server.Utils.Email;

namespace server.Services;

public class EmailService(IOptions<MailSettings> mailSettings, IConfiguration configuration) : IEmailService
{
    private readonly MailSettings _mailSettings = mailSettings.Value;
    private readonly IConfiguration configuration = configuration;

    public async Task SendEmailAsync(MailData mailData, BodyBuilder bodyBuilder)
    {
        MimeMessage emailMessage = new();
        MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
        emailMessage.From.Add(emailFrom);
        MailboxAddress emailTo = new(mailData.EmailToName, mailData.EmailToId);
        emailMessage.To.Add(emailTo);
        emailMessage.Subject = mailData.EmailSubject;
        emailMessage.Body = bodyBuilder.ToMessageBody();

        SmtpClient client = new();
        await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    public async Task SendForgotPasswordEmailAsync(string email, string username, string token)
    {
        try
        {
            string verificationLink = configuration["URL"] + $"forgot-password?token={token}&email={email}";

            MailData data = new()
            {
                EmailToId = email,
                EmailToName = username,
                EmailSubject = "Password Reset Request",
                EmailBody = $"Hi {username},\nForgot your password?\n" +
                $"We received a request to reset the password for your account\n" +
                $"To reset your password, click on the button below:\n" +
                $"{verificationLink}",
            };

            string filePath = GetTemplatePath("ForgotPassword.html");
            string emailTemplateText = File.ReadAllText(filePath);

            emailTemplateText = emailTemplateText.Replace("{{Name}}", data.EmailToName);
            emailTemplateText = emailTemplateText.Replace("{{Link}}", verificationLink);

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = emailTemplateText,
                TextBody = data.EmailBody
            };

            await SendEmailAsync(data, bodyBuilder);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task SendVerificationEmailAsync(string email, string token)
    {
        try
        {
            string verificationLink = configuration["URL"] + $"verify-email?token={token}&email={email}";

            MailData data = new()
            {
                EmailToId = email,
                EmailToName = Guid.NewGuid().ToString(),
                EmailSubject = "Email Verification",
                EmailBody = $"<h1>Email Verification</h1><p>Click <a href='{verificationLink}'>here</a> to verify your email.</p>"
            };

            string filePath = GetTemplatePath("EmailVerification.html");
            string emailTemplateText = File.ReadAllText(filePath);

            emailTemplateText = emailTemplateText.Replace("{{name}}", data.EmailToName);
            emailTemplateText = emailTemplateText.Replace("{{link}}", verificationLink);

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = emailTemplateText,
                TextBody = data.EmailBody
            };

            await SendEmailAsync(data, bodyBuilder);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string GetTemplatePath(string templateName)
    {
        var pathTemp = configuration.GetValue<string>("TemplatePath");
        string templatePath = Path.Combine(pathTemp!, templateName);
        return templatePath;
    }
}

