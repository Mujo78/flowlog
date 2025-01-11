using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace server.Data;

public class ApplicationDBContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUser>().Ignore(c => c.LockoutEnabled)
                                           .Ignore(c => c.PhoneNumber)
                                           .Ignore(c => c.PhoneNumberConfirmed)
                                           .Ignore(c => c.LockoutEnd)
                                           .Ignore(c => c.TwoFactorEnabled);

        modelBuilder.Entity<IdentityUser>().ToTable("Users");

    }
}
