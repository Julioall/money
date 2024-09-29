using Application.Domain.Model;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AccountService _accountService;
        private readonly AuthenticationService _authenticationService;

        public AccountController(
            UserService userService,
            AccountService accountService,
            AuthenticationService authenticationService
        )
        {
            _userService = userService;
            _accountService = accountService;
            _authenticationService = authenticationService;
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> Add([FromBody] Account account)
        {
            if (account == null)
                return BadRequest("Account data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var createdAccount = await _accountService.AddAsync(userId, account);
            return CreatedAtAction(nameof(GetById), new { accountId = createdAccount.Id }, createdAccount);
        }

        [HttpDelete("accounts/{accountId}")]
        public async Task<IActionResult> Delete(string accountId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var success = await _accountService.DeleteAsync(userId, accountId);
            if (!success)
                return NotFound("Account not found.");

            return NoContent();
        }

        [HttpPut("accounts")]
        public async Task<IActionResult> Update([FromBody] Account account)
        {
            if (account == null)
                return BadRequest("Account data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedAccount = await _accountService.UpdateAsync(userId, account);
            return Ok(updatedAccount);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var accounts = await _accountService.GetAllAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("accounts/{accountId}")]
        public async Task<IActionResult> GetById(string accountId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var account = await _accountService.GetByIdAsync(userId, accountId);
            if (account == null)
                return NotFound("Account not found.");
                
            return Ok(account);
        }
    }
}
