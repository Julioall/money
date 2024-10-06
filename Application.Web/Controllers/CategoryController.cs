using Application.Domain.Model;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly AuthenticationService _authenticationService;

        public CategoryController(
            UserService userService,
            CategoryService categoryService,
            AuthenticationService authenticationService
        )
        {
            _userService = userService;
            _categoryService = categoryService;
            _authenticationService = authenticationService;
        }

        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("Category data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var createdCategory = await _categoryService.AddAsync(userId, category);
            return CreatedAtAction(nameof(GetById), new { categoryId = createdCategory.Id }, createdCategory);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(string categoryId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            
            var  success = await _categoryService.DeleteAsync(userId, categoryId);
            if (!success)
                return NotFound("Category not found.");

            return NoContent();
        }

        [HttpPut()]
        public async Task<IActionResult> Updatey([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("Category data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedCategory = await _categoryService.UpdateAsync(userId, category);
            return Ok(updatedCategory);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var categories = await _categoryService.GetAllAsync(userId);
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById(string categoryId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var category = await _categoryService.GetByIdAsync(userId, categoryId);
            if (category == null)
                return NotFound("Category not found.");
                
            return Ok(category);
        }
    }
}
