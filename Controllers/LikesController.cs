﻿using AutoMapper;
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
    public class LikesController(ILikeRepository likeRepository, IMapper mapper) : ControllerBase
    {
        private readonly ILikeRepository likeRepository = likeRepository;

        [HttpPost]
        [Route("{tweetId}")]
        [Authorize]
        public async Task<IActionResult> LikeToggle([FromRoute] Guid tweetId)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if(userId == null)
            {
                return BadRequest("You must log in in order to like this post.");
            }

            var isLikedDomain = await likeRepository.LikeToggle(tweetId, userId);

            if(isLikedDomain == null)
            {
                return Ok(false);
            }

            var likeDTO = mapper.Map<LikeDTO>(isLikedDomain);

            return Ok(likeDTO);
        }

    }
}
