
namespace server.Utils.Email;

public class MailSettings
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
