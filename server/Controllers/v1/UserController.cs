using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using server.DTO.User;

namespace server.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("signup")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            if (signUpDTO == null) return BadRequest("Please provide valid data for registration.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok("Success");
        }
    }
}
