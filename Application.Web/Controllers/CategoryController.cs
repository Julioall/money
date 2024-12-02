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
    public class CategoryController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly AuthenticationService _authenticationService;
        private readonly IStringLocalizer<Messages> _localizer;

        public CategoryController(
            UserService userService,
            CategoryService categoryService,
            AuthenticationService authenticationService,
            IStringLocalizer<Messages> localizer
        )
        {
            _userService = userService;
            _categoryService = categoryService;
            _authenticationService = authenticationService;
            _localizer = localizer;
        }

        [HttpPost()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (category == null)
                return BadRequest(_localizer["CategoryCannotBeNull"].Value);

            var userId = await _authenticationService.GetUserIdLoged();
            var createdCategory = await _categoryService.AddAsync(userId, category);
            return CreatedAtAction(nameof(GetById), new { categoryId = createdCategory.Id }, createdCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();

            var success = await _categoryService.DeleteAsync(userId, id);
            if (!success)
                return NotFound(_localizer["CategoryNotFoundById", id].Value);

            return NoContent();
        }

        [HttpPut()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Updatey([FromBody] Category category)
        {
            if (category == null)
                return BadRequest(_localizer["CategoryCannotBeNull"].Value);

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedCategory = await _categoryService.UpdateAsync(userId, category);
            return Ok(updatedCategory);
        }

        [HttpGet()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var categories = await _categoryService.GetAllAsync(userId);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var category = await _categoryService.GetByIdAsync(userId, id);
            if (category == null)
                return NotFound(_localizer["UserCannotBeNull"].Value);

            return Ok(category);
        }
    }
}
