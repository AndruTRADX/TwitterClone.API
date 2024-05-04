using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Models.DTOs;
using TwitterClone.Repositories;

namespace TwitterClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);

            if (identityResult.Succeeded)
            {
                return Ok(new { message = "User registered successfully. Please login." });
            }

            return BadRequest(new { message = "Failed to register user." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user != null)
            {
                var passwordIsValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                if (passwordIsValid)
                {
                    var jwtToken = tokenRepository.CreateJWTToken(user);
                    var response = new LoginResponseDTO
                    {
                        JwtToken = jwtToken,
                    };
                    return Ok(response);
                }
            }

            return BadRequest(new { message = "Email or password incorrect, please check your credentials :)" });
        }
    }
}
