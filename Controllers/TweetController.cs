using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TwitterClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TweetController : ControllerBase
    {
        [HttpGet]
        [Route("users/current")]
        public async Task<IActionResult> GetLoggerUserTweets()
        {
            var id = HttpContext.User.FindFirstValue("UserId");
            var userName = HttpContext.User.FindFirstValue("UserName");

            if (id == null && userName == null)
            {
                return Ok("XD");
            }

            return Ok(new { id, userName });
        }
    }
}
