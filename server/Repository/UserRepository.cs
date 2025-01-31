using System;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repository.IRepository;

namespace server.Repository;

public class UserRepository(ApplicationDBContext db) : Repository<ApplicationUser>(db), IUserRepository
{
    private readonly ApplicationDBContext db = db;

    public async Task CreateUserEmailToken(EmailVerificationToken emailVerificationToken)
    {
        await db.EmailVerificationTokens.AddAsync(emailVerificationToken);
        await db.SaveChangesAsync();
    }

    public async Task<EmailVerificationToken?> GetEmailToken(string token)
    {
        return await db.EmailVerificationTokens.FirstOrDefaultAsync(n => n.Token.Equals(token));
    }

    public async Task VerifyEmailToken(EmailVerificationToken token)
    {
        db.EmailVerificationTokens.Update(token);
        await db.SaveChangesAsync();
    }

    public bool EmailAlreadyUsed(string email)
    {
        return db.Users.Any(user => user.Email!.Equals(email));
    }

    public bool IsEmailTaken(string email)
    {
        return db.EmailVerificationTokens.Any(token => token.Email.Equals(email));
    }
}
