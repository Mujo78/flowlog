using System;
using server.Data;
using server.Models;
using server.Repository.IRepository;

namespace server.Repository;

public class UserRepository(ApplicationDBContext db) : Repository<ApplicationUser>(db), IUserRepository
{
    private readonly ApplicationDBContext dBContext = db;

    public async Task CreateUserEmailToken(EmailVerificationToken emailVerificationToken)
    {
        await dBContext.EmailVerificationTokens.AddAsync(emailVerificationToken);
        await dBContext.SaveChangesAsync();
    }

    public bool EmailAlreadyUsed(string email)
    {
        return dBContext.Users.Any(user => user.Email!.Equals(email));
    }

    public bool IsEmailTaken(string email)
    {
        return dBContext.EmailVerificationTokens.Any(token => token.Email.Equals(email));
    }
}
