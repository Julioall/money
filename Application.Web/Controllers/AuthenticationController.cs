using Application.Domain.Dtos;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login information is missing.");
            }

            var token = await _authenticationService.GenerateToken(loginDto);
            await _authenticationService.RegisterLastLoginAsync(loginDto);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid login credentials.");
            }

            return Ok(new { Token = token });
        }
    }
}
