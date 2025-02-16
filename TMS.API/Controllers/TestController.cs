using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.API.Controllers
{
    [Route("api/test")]
    [ApiController]

    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet()]
        public object TestMessage()
        {
            return Ok(new { message = "You have logined to server", timestamp = DateTime.Now });
        }
        [HttpGet("public")]
        public object TestPublicMessage()
        {
            return Ok(new { message = "Test message from server", timestamp = DateTime.Now });
        }
    }
}
