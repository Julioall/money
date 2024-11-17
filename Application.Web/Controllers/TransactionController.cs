using Application.Domain.Model;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly AuthenticationService _authenticationService;

        public TransactionController(
            TransactionService transactionService,
            AuthenticationService authenticationService)
        {
            _transactionService = transactionService;
            _authenticationService = authenticationService;
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> Add([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction data is null.");

            var createdTransaction = await _transactionService.AddAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { transactionId = createdTransaction.Id }, createdTransaction);
        }

        [HttpDelete("transactions/{transactionId}")]
        public async Task<IActionResult> Delete(string transactionId)
        {
            var userId = await _authenticationService.GetUserIdLoged();

            var success = await _transactionService.DeleteAsync(userId, transactionId);
            if (!success)
                return NotFound("Transaction not found.");

            return NoContent();
        }

        [HttpPut("transactions")]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction data is null.");

            var updatedTransaction = await _transactionService.UpdateAsync(transaction);
            return Ok(updatedTransaction);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var transactions = await _transactionService.GetAllAsync(userId);
            return Ok(transactions);
        }

        [HttpGet("transactions/{transactionId}")]
        public async Task<IActionResult> GetById(string transactionId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var transaction = await _transactionService.GetByIdAsync(userId, transactionId);
            if (transaction == null)
                return NotFound("Transaction not found.");

            return Ok(transaction);
        }
    }
}
