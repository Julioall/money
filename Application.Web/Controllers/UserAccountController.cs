using Application.Service.Dtos;
using Application.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public UserAccountController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login information is missing.");
            }

            var token = await _tokenService.GenerateToken(loginDto);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid login credentials.");
            }

            return Ok(new { Token = token });
        }
    }
}
