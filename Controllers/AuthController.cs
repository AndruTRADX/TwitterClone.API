using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;
using TwitterClone.Repositories;

namespace TwitterClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerRequestDTO)
        {
            var identityUser = new ApplicationUser
            {
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Biography = registerRequestDTO.Biography,
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                return Ok("User was registered! Please login.");
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var jwtToken = tokenRepository.CreateJWTToken(user);

                    var response = new LoginResponseDTO
                    {
                        JwtToken = jwtToken
                    };

                    return Ok(response);
                }
            }

            return BadRequest("User name or password incorrect");
        }
    }
}
