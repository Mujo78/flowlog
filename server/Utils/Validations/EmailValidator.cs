
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using server.Models;

namespace server.Utils.Validations;

public class EmailValidator(params string[]? domains) : ValidationAttribute, IUserValidator<ApplicationUser>
{
    private readonly string[] _domains = domains ?? ["admin", "root", "superviser", "example", "company"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email)
        {
            var userEmail = email.Split('@');
            var emailDomain = userEmail.LastOrDefault();
            if (!string.IsNullOrEmpty(emailDomain) && _domains.Contains(email, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Email with that domain is not acceptable.");
            }

            var emailUser = userEmail.FirstOrDefault();
            if (!string.IsNullOrEmpty(emailUser) && _domains.Contains(emailUser, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult("Email contains forbbiden words.");
            }

        }

        return ValidationResult.Success;
    }
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var userEmail = user.Email?.Split('@');
        var emailDomain = userEmail?.LastOrDefault();
        if (!string.IsNullOrEmpty(emailDomain) && _domains.Contains(emailDomain, StringComparer.OrdinalIgnoreCase))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidEmailDomain",
                Description = "Email with that domain is not acceptable."
            }));
        }

        var emailUser = userEmail?.FirstOrDefault();

        if (!string.IsNullOrEmpty(emailUser) && _domains.Contains(emailUser, StringComparer.OrdinalIgnoreCase))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "ForbbidenEmailName",
                Description = "Email contains forbbiden words."
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}
