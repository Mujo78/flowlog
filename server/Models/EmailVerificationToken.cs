using System;

namespace server.Models;

public class EmailVerificationToken
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationTime { get; set; }
}