using BlogAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

// healthCheck
namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ApiKey]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}