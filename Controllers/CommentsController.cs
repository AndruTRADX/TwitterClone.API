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
    public class CommentsController(ICommentRepository commentRepository) : ControllerBase
    {
        private readonly ICommentRepository commentRepository = commentRepository;

        [HttpPost]
        [Route("{tweetId:Guid}")]
        [Authorize]
        public async Task<IActionResult> PostCommentToTweet([FromRoute] Guid tweetId,[FromBody] SubmitCommentDTO commentDTO)
        {
            var userName = HttpContext.User.FindFirstValue("UserName");
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userName == null || userId == null)
            {
                return BadRequest("The request has not been processed, try again.");
            }

            var comment = await commentRepository.PostCommentToTweetAsync(commentDTO, userName, userId, tweetId);

            if (comment == null)
            {
                NotFound("Tweet has not been round.");
            }

            return Ok(comment);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> PostCommentToTweet([FromRoute] Guid id)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userId == null)
            { 
                return Unauthorized("Request has not been processed, please sign in.");
            }

            var comment = await commentRepository.DeleteCommentFromTweetAsync(id, userId);

            if (comment == null)
            {
                NotFound("Tweet has not been round.");
            }

            return Ok(comment);
        }
    }
}
