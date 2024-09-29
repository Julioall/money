using Application.Domain.Model;
using Application.Service.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            if (user is null || loginDto is null || user?.Account?.Email != loginDto?.Email || user?.Account?.Password != loginDto?.Password)
            {
                return string.Empty;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AplicationConstants.JWT_SIGNING_KEY ?? string.Empty));
            var signinCredentials = new SigningCredentials(secretKey, algorithm: SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: AplicationConstants.JWT_ISSUER,
                audience: AplicationConstants.JWT_AUDIENCE,
                claims:
                [
                    new Claim(type: ClaimTypes.NameIdentifier, user.Id),
                    new Claim(type: ClaimTypes.Name, user.Account.Email),
                    new Claim(type: ClaimTypes.Role, user.Account.Role)
                ],
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signinCredentials
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }

        public async Task<string> GetUserIdLoged()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new Exception("User is not authenticated");

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userInDataBase = await _userService.GetByIdAsync(userId);
            if (userInDataBase == null)
                throw new Exception("User loged not found in database");
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID claim not found");

            return userId;
        }
    }
}
