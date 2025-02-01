using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class ApplicationDBContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>().Ignore(c => c.LockoutEnabled)
                                           .Ignore(c => c.PhoneNumber)
                                           .Ignore(c => c.PhoneNumberConfirmed)
                                           .Ignore(c => c.LockoutEnd)
                                           .Ignore(c => c.TwoFactorEnabled);

        modelBuilder.Entity<ApplicationUser>().ToTable("Users");

        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new ApplicationRole()
            {
                Id = Guid.NewGuid(),
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
}
