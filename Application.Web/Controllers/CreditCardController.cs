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

        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] CreditCard creditCard)
        {
            if (creditCard == null)
                return BadRequest("Credit card data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var createdCreditCard = await _creditCardService.AddAsync(userId, creditCard);
            return CreatedAtAction(nameof(GetById), new { creditCardId = createdCreditCard.Id }, createdCreditCard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var success = await _creditCardService.DeleteAsync(userId, id);
            if (!success)
                return NotFound("Credit card not found.");

            return NoContent();
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] CreditCard creditCard)
        {
            if (creditCard == null)
                return BadRequest("Credit card data is null.");

            var userId = await _authenticationService.GetUserIdLoged();
            var updatedCreditCard = await _creditCardService.UpdateAsync(userId, creditCard);
            return Ok(updatedCreditCard);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var creditCards = await _creditCardService.GetAllAsync(userId);
            return Ok(creditCards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = await _authenticationService.GetUserIdLoged();
            var creditCard = await _creditCardService.GetByIdAsync(userId, id);
            if (creditCard == null)
                return NotFound("Credit card not found.");

            return Ok(creditCard);
        }
    }
}
