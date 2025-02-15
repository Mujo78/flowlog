using System;
using System.ComponentModel.DataAnnotations;

namespace server.Utils.Validations;

public class EmailOrUsernameValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string emailOrUsername)
        {
            if (emailOrUsername.Contains('@'))
            {
                var email = new EmailAddressAttribute();
                if (!email.IsValid(emailOrUsername))
                {
                    return new ValidationResult("Please provide valid email address.");
                }
            }
        }

        return ValidationResult.Success;
    }
}
