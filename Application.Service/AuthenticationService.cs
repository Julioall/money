using Application.Domain.Dtos;
using Application.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Application.Service
{
    public class AuthenticationService
    {
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GenerateToken(LoginDto loginDto)
        {
            var user = await _userService.GetByEmailAsync(loginDto.Email);
            
            if (user == null || loginDto == null || user.Account.Email != loginDto.Email || user.Account.Password != loginDto.Password)
                return string.Empty;
            
            if (string.IsNullOrEmpty(ApplicationConstants.JWT_SIGNING_KEY))
                throw new Exception("JWT signing key is not configured.");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConstants.JWT_SIGNING_KEY));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Account.Email),
                new Claim(ClaimTypes.Role, user.Account.Role)
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: ApplicationConstants.JWT_ISSUER,
                audience: ApplicationConstants.JWT_AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<string> GetUserIdLoged()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("User ID claim not found.");

            var userInDatabase = await _userService.GetByIdAsync(userId);
            if (userInDatabase == null)
                throw new Exception("User logged not found in database.");

            return userId;
        }

        public async Task RegisterLastLoginAsync(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email))
                throw new ArgumentNullException(nameof(loginDto.Email), "User E-mail cannot be null or empty.");

            var user = await _userService.GetByEmailAsync(loginDto.Email);
            if (user == null)
                throw new KeyNotFoundException($"User with E-mail {loginDto.Email} not found.");

            user.Account.LastLogin = DateTime.UtcNow;
            user.Account.LastLoginOperatingSystem = loginDto.OperatingSystem;
            user.Account.LastLoginBrowser = loginDto.Browser;
            await _userService.UpdateAsync(user);
        }
    }
}
