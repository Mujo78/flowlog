using System;
using server.Models;

namespace server.Repository.IRepository;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task CreateUserEmailToken(EmailVerificationToken emailVerificationToken);
    bool EmailAlreadyUsed(string email);
    bool IsEmailTaken(string email);
}
