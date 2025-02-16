using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using server.DTO.Auth;
using server.Services.IService;

namespace server.Controllers
{
    [Route("api/auth/")]
    [ApiVersionNeutral]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null) return BadRequest("Please provide valid data for login.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _authService.Login(loginDTO);
            return Ok();
        }
    }
}
