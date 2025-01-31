using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using server.DTO.User;
using server.Services.IService;

namespace server.Controllers
{
    [Route("api/v{version:apiVersion}/users/")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("signup")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            if (signUpDTO == null) return BadRequest("Please provide valid data for registration.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _userService.UserSignup(signUpDTO);
            return Ok("Success");
        }

        [HttpPost("verify-email/{token}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> VerifyEmail([FromRoute] string token)
        {
            if (token == null) return BadRequest("Please provide valid data for email verification.");

            await _userService.VerifyEmail(token);
            return Ok("Success");
        }
    }
}
