using System;
using server.Models;

namespace server.Repository.IRepository;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task CreateUserEmailToken(EmailVerificationToken emailVerificationToken);
    Task<EmailVerificationToken?> GetEmailToken(string token);
    Task<EmailVerificationToken?> GetEmailTokenByEmail(string email);
    Task VerifyEmailToken(EmailVerificationToken token);
    Task<Guid?> GetRoleByName(string name);
    Task DeleteUserEmailToken(EmailVerificationToken emailVerificationToken);
    bool EmailAlreadyUsed(string email);
    bool IsEmailTaken(string email);
}
