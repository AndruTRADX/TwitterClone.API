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
    public class CommentsController(ICommentRepository commentRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICommentRepository commentRepository = commentRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet("{tweetId}")]
        public async Task<IActionResult> GetCommentsForTweet(Guid tweetId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var comments = await commentRepository.GetCommentsForTweetAsync(tweetId, pageNumber, pageSize);

            return Ok(comments);
        }

        [HttpPost]
        [Route("{tweetId:Guid}")]
        [Authorize]
        public async Task<IActionResult> PostCommentToTweet([FromRoute] Guid tweetId, [FromBody] SubmitCommentDTO submitCommentDTO)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userId == null)
            {
                return BadRequest("The request has not been processed, try again.");
            }

            var commentDomain = await commentRepository.PostCommentToTweetAsync(submitCommentDTO, userId, tweetId);

            if (commentDomain == null)
            {
                return NotFound("Tweet not found.");
            }

            var commentDTO = mapper.Map<CommentDTOCreated>(commentDomain);

            return Ok(commentDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userId == null)
            {
                return Unauthorized("Request has not been processed, please sign in.");
            }

            var commentDomain = await commentRepository.DeleteCommentFromTweetAsync(id, userId);

            if (commentDomain == null)
            {
                return NotFound("Comment not found.");
            }

            var commentDTO = mapper.Map<CommentDTOCreated>(commentDomain);

            return Ok(commentDTO);
        }
    }
}
