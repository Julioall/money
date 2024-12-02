using Application.Domain.Model;
using Application.Service;
using Application.Web.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly IStringLocalizer<Messages> _localizer;

        public UserController(
            UserService userService,
            CategoryService categoryService,
            IStringLocalizer<Messages> localizer
        )
        {
            _userService = userService;
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Add(User user)
        {
            if (user == null)
                return BadRequest(_localizer["UserCannotBeNull"].Value);

            var userDoBanco = await _userService.GetByEmailAsync(user.Account.Email);
            if (userDoBanco != null)
                return BadRequest(_localizer["EmailAlreadyRegistered"].Value);

            var createdUser = await _userService.AddAsync(user);
            _categoryService.AddDefaultCategoryAsync(createdUser.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, new { message = _localizer["UserAddedSuccessfully"].Value });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(_localizer["IdCannotBeNullOrEmpty"].Value);

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(_localizer["UserNotFoundById", id].Value);

            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(_localizer["IdCannotBeNullOrEmpty"].Value);

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(_localizer["UserNotFoundById", id].Value);

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-by-role/{role}")]
        public async Task<ActionResult<IEnumerable<User>>> GetByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return BadRequest(_localizer["RoleCannotBeNullOrEmpty"].Value);

            var users = await _userService.GetByRoleAsync(role);
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-by-email/{email}")]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(_localizer["EmailCannotBeNullOrEmpty"].Value);

            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound(_localizer["UserNotFoundByEmail", email].Value);

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut()]
        public async Task<IActionResult> Update(User user)
        {
            if (user == null)
                return BadRequest(_localizer["UserCannotBeNull"].Value);

            var updatedUser = await _userService.UpdateAsync(user);
            return Ok(new { message = _localizer["UserUpdatedSuccessfully"].Value, user = updatedUser });
        }
    }
}
