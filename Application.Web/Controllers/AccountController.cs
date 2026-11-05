using Application.Domain.Models;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
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

        private static bool IsValidAccount(Account account)
        {
            var name = account.AccountName?.Trim();
            return name is { Length: >= 3 and <= 100 }
                && account.Balance >= 0
                && !string.IsNullOrWhiteSpace(account.IconUrl)
                && (account.BankName == null || account.BankName.Length <= 100)
                && (account.BankIspb == null ||
                    (account.BankIspb.Length == 8 && account.BankIspb.All(char.IsDigit)))
                && (account.AccountType == null ||
                    new[] { "checking", "savings", "digital", "wallet", "investment", "other" }.Contains(account.AccountType));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Add([FromBody] Account account)
        {
            if (account == null)
                return BadRequest("Account data is null.");

            if (!IsValidAccount(account))
                return BadRequest("Invalid account. Enter a name (3-100 characters), a nonnegative balance and an icon.");

            var userId = await _authenticationService.GetUserIdLoged();
            var createdAccount = await _accountService.AddAsync(userId, account);
            return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var success = await _accountService.DeleteAsync(userId, id);
            if (!success)
                return NotFound("Account not found.");

            return NoContent();
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Account account)
        {
            if (account == null)
                return BadRequest("Account data is null.");

            if (string.IsNullOrWhiteSpace(account.Id) || !IsValidAccount(account))
                return BadRequest("Invalid account data.");

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedAccount = await _accountService.UpdateAsync(userId, account);
            return Ok(updatedAccount);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var accounts = await _accountService.GetAllAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var account = await _accountService.GetByIdAsync(userId, id);
            if (account == null)
                return NotFound("Account not found.");

            return Ok(account);
        }
    }
}
