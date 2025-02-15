using Microsoft.AspNetCore.Identity;
using server.DTO.Auth;
using server.Models;
using server.Services.IService;

namespace server.Services;

public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task Login(LoginDTO loginDTO)
    {
        var user = await userManager.FindByEmailAsync(loginDTO.EmailOrUsername) ?? await userManager.FindByNameAsync(loginDTO.EmailOrUsername);
        var isValid = await userManager.CheckPasswordAsync(user!, loginDTO.Password);

        Console.WriteLine(isValid);
    }
}
