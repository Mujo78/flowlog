using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/Auth")]
    [ApiVersionNeutral]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpGet("/")]
        public async Task<ActionResult> GetUser()
        {
            return Ok();
        }
    }
}
