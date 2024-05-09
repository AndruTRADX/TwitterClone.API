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
    public class TweetsController(ITweetRepository tweetRepository, IMapper mapper) : ControllerBase
    {
        private readonly ITweetRepository tweetRepository = tweetRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAllTweets([FromQuery] int size = 10, [FromQuery] int offset = 1) {
            var tweetDTOs = await tweetRepository.GetAllTweetsAsync(size, offset);
            return Ok(tweetDTOs);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetTweet([FromRoute] Guid id)
        {
            var tweetDomain = await tweetRepository.GetTweetAsync(id);

            if (tweetDomain == null)
            {
                return NotFound("Your tweet was not found.");
            }

            var tweetDTO = mapper.Map<TweetDTO>(tweetDomain);

            return Ok(tweetDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTweet([FromBody] SubmitTweetDTO submitTweetDTO)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var firstName = HttpContext.User.FindFirstValue("FirstName");
            var userName = HttpContext.User.FindFirstValue("UserName");

            if (userName == null || userId == null || firstName == null) 
            {
                return BadRequest("The request has not been processed, try again.");
            }

            var tweetDomain = await tweetRepository.CreateTweetAsync(submitTweetDTO, userName, firstName, userId);
            var tweetDTO = mapper.Map<TweetDTO>(tweetDomain);

            return Ok(tweetDTO);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateTweet([FromBody] SubmitTweetDTO submitTweetDTO, [FromRoute] Guid id)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if(userId == null)
            {
                return Unauthorized("Request was not processed, please sign in.");
            }

            var tweetUpdatedDomain = await tweetRepository.UpdateTweetAsync(submitTweetDTO, id, userId);

            if (tweetUpdatedDomain == null)
            {
                return BadRequest("Your tweet was not found.");
            }

            var tweetDTO = mapper.Map<TweetDTO>(tweetUpdatedDomain);

            return Ok(tweetDTO);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteTweet([FromRoute] Guid id)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userId == null)
            {
                return Unauthorized("Request was not process, please sign in.");
            }

            var tweetRemovedDomain = await tweetRepository.DeleteTweetAsync(id, userId);

            if (tweetRemovedDomain == null)
            {
                return BadRequest("Your tweet was not found.");
            }

            var tweetDTO = mapper.Map<TweetDTO>(tweetRemovedDomain);

            return Ok(tweetDTO);
        }
    }
}
