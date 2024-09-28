using Application.Domain.Interface;
using Application.Domain.Model;

namespace Application.Service
{
    public class CreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public CreditCardService(ICreditCardRepository creditCardRepository)
        {
            _creditCardRepository = creditCardRepository;
        }

        public async Task<CreditCard> CreateAsync(string userId, CreditCard creditCard)
        {
            creditCard.UserId = userId;
            await _creditCardRepository.AddAsync(userId, creditCard);
            return creditCard;
        }

        public async Task<bool> DeleteAsync(string userId, string creditCardId)
        {
            await _creditCardRepository.DeleteAsync(userId, creditCardId);
            return true;
        }

        public async Task<CreditCard> UpdateAsync(string userId, CreditCard creditCard)
        {
            await _creditCardRepository.UpdateAsync(userId, creditCard);
            return creditCard;
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync(string userId)
        {
            return await _creditCardRepository.GetAllAsync(userId);
        }

        public async Task<CreditCard> GetByIdAsync(string userId, string creditCardId)
        {
            return await _creditCardRepository.GetByIdAsync(userId, creditCardId);
        }
    }
}
