using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        { }

        [HttpGet("GetToken", Name = "GetToken")]
        public async Task<ActionResult<string>> Get(UserInfo user)
        {
            return Ok(new { data = string.Concat(user.User, user.Password) });
        }
    }

    public class UserInfo
    {
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
