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

        public async Task<CreditCard> AddAsync(string userId, CreditCard creditCard)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (creditCard == null)
                throw new ArgumentNullException(nameof(creditCard), "CreditCard cannot be null.");

            creditCard.UserId = userId;
            await _creditCardRepository.AddAsync(userId, creditCard);
            return creditCard;
        }

        public async Task<bool> DeleteAsync(string userId, string creditCardId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(creditCardId))
                throw new ArgumentNullException(nameof(creditCardId), "Credit Card ID cannot be null or empty.");

            await _creditCardRepository.DeleteAsync(userId, creditCardId);
            return true;
        }

        public async Task<CreditCard> UpdateAsync(string userId, CreditCard creditCard)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (creditCard == null)
                throw new ArgumentNullException(nameof(creditCard), "Credit Card cannot be null.");

            await _creditCardRepository.UpdateAsync(userId, creditCard);
            return creditCard;
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            return await _creditCardRepository.GetAllAsync(userId);
        }

        public async Task<CreditCard> GetByIdAsync(string userId, string creditCardId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(creditCardId))
                throw new ArgumentNullException(nameof(creditCardId), "Credit Card ID cannot be null or empty.");

            return await _creditCardRepository.GetByIdAsync(userId, creditCardId);
        }
    }
}
