using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace server;

public class ResetPasswordTokenProviderCustom<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
    public ResetPasswordTokenProviderCustom(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<ResetPasswordTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
    {
    }
}


public class ResetPasswordTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public ResetPasswordTokenProviderOptions()
    {
        Name = "ResetPasswordDataProtectorTokenProvider";
        TokenLifespan = TimeSpan.FromHours(3);
    }
}