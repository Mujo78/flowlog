using System;
using server.DTO.Auth;

namespace server.Services.IService;

public interface IAuthService
{
    Task Login(LoginDTO loginDTO);
}
