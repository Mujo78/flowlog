using System;
using server.Models;

namespace server.Repository.IRepository;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task CreateUserEmailToken(EmailVerificationToken emailVerificationToken);
    Task<EmailVerificationToken?> GetEmailToken(string token);
    Task VerifyEmailToken(EmailVerificationToken token);
    bool EmailAlreadyUsed(string email);
    bool IsEmailTaken(string email);
}
