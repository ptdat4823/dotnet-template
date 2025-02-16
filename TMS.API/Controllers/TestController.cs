using Microsoft.AspNetCore.Mvc;

namespace TMS.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet()]
        public object TestMessage()
        {
            return Ok(new { message = "New message from server", timestamp = DateTime.Now });
        }
    }
}
