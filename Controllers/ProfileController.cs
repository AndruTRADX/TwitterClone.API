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
    public class ProfileController(IProfileRepository profileRepository, IMapper mapper) : ControllerBase
    {
        private readonly IProfileRepository profileRepository = profileRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userName)
        {
            var userDomain = await profileRepository.GetUserProfileAsync(userName);
            
            if (userDomain == null)
            {
                return NotFound("User was not found");
            }

            var userDTO = mapper.Map<ApplicationUserDTO>(userDomain);

            return Ok(userDTO);
        }

        [HttpGet]
        [Route("tweets/{userName}")]
        public async Task<IActionResult> GetUserTweets([FromRoute] string userName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userTweets = await profileRepository.GetAllUserTweetsAsync(userName, pageNumber, pageSize);

            if (userTweets == null)
            {
                return NotFound("User was not found");
            }

            return Ok(userTweets);
        }

        [HttpGet]
        [Route("users/{search}")]
        public async Task<IActionResult> GetUsersProfile([FromRoute] string search)
        {
            var usersDomain = await profileRepository.SearchUsersAsync(search);

            if (usersDomain == null || usersDomain.Count == 0)
            {
                return NotFound("User was not found");
            }

            var usersDTO = mapper.Map<List<ApplicationUserDTO>>(usersDomain);

            return Ok(usersDTO);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] SubmitProfileDTO profileInfo)
        {
            var userId = HttpContext.User.FindFirstValue("UserId");

            if (userId == null)
            {
                return BadRequest("You must log into your account in order to update your profile.");
            }

            var updatedProfileDomain = await profileRepository.UpdateUserProfileAsync(profileInfo, userId);
            var updatedProfileDTO = mapper.Map<ApplicationUserDTO>(updatedProfileDomain);

            return Ok(updatedProfileDTO);
        }
    }
}
