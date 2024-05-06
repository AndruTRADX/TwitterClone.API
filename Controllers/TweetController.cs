using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterClone.Models.DTOs;
using TwitterClone.Repositories;

namespace TwitterClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController(ITweetRepository tweetRepository, IMapper mapper) : ControllerBase
    {
        private readonly ITweetRepository tweetRepository = tweetRepository;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetLoggerUserTweets([FromBody] SubmitTweetDTO submitTweetDTO)
        {
            var userName = HttpContext.User.FindFirstValue("UserName");
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userName == null || userId == null) 
            {
                return BadRequest("The request has not been processed, try again.");
            }

            var tweetDomain = await tweetRepository.CreateTweetAsync(submitTweetDTO, userName, userId);
            var tweetDTO = mapper.Map<TweetDTO>(tweetDomain);

            return Ok(tweetDTO);
        }
    }
}
