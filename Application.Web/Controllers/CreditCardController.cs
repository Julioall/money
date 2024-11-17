using Application.Domain.Model;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreditCardController : ControllerBase
    {
        private readonly CreditCardService _creditCardService;
        private readonly AuthenticationService _authenticationService;

        public CreditCardController(
            CreditCardService creditCardService,
            AuthenticationService authenticationService
        )
        {
            _creditCardService = creditCardService;
            _authenticationService = authenticationService;
        }

        [HttpPost("creditcards")]
        public async Task<IActionResult> Add([FromBody] CreditCard creditCard)
        {
            if (creditCard == null)
                return BadRequest("Credit card data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var createdCreditCard = await _creditCardService.AddAsync(userId, creditCard);
            return CreatedAtAction(nameof(GetById), new { creditCardId = createdCreditCard.Id }, createdCreditCard);
        }

        [HttpDelete("creditcards/{creditCardId}")]
        public async Task<IActionResult> Delete(string creditCardId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var success = await _creditCardService.DeleteAsync(userId, creditCardId);
            if (!success)
                return NotFound("Credit card not found.");

            return NoContent();
        }

        [HttpPut("creditcards")]
        public async Task<IActionResult> Update([FromBody] CreditCard creditCard)
        {
            if (creditCard == null)
                return BadRequest("Credit card data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedCreditCard = await _creditCardService.UpdateAsync(userId, creditCard);
            return Ok(updatedCreditCard);
        }

        [HttpGet("creditcards")]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var creditCards = await _creditCardService.GetAllAsync(userId);
            return Ok(creditCards);
        }

        [HttpGet("creditcards/{creditCardId}")]
        public async Task<IActionResult> GetById(string creditCardId)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var creditCard = await _creditCardService.GetByIdAsync(userId, creditCardId);
            if (creditCard == null)
                return NotFound("Credit card not found.");

            return Ok(creditCard);
        }
    }
}
