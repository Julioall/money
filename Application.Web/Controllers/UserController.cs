using Application.Domain.Model;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> Add(User user)
        {
            if (user == null)
                return BadRequest("User cannot be null.");

            var userDoBanco = await _userService.GetByEmailAsync(user.Account.Email);
            if (userDoBanco != null)
                return BadRequest("This email is already registered.");

            var createdUser = await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, new { message = "User added successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<User>>> GetByRole(string role)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email cannot be null or empty.");
            var users = await _userService.GetByRoleAsync(role);
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("username/{email}")]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email cannot be null or empty.");

            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(User user)
        {
            if (user == null)
                return BadRequest("User cannot be null.");

            var existingUser = await _userService.GetByIdAsync(user.Id);
            if (existingUser == null)
                return NotFound($"User with ID {user.Id} not found.");

            var updatedUser = await _userService.UpdateAsync(user);
            return Ok(new { message = "User updated successfully.", user = updatedUser });
        }
    }
}
