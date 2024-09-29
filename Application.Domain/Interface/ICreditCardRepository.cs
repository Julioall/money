using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface ICreditCardRepository
    {
        Task<CreditCard> AddAsync(string userId, CreditCard creditCard);
        Task<bool> DeleteAsync(string userId, string creditCardId);
        Task<CreditCard> UpdateAsync(string userId, CreditCard creditCard);
        Task<IEnumerable<CreditCard>> GetAllAsync(string userId);
        Task<CreditCard> GetByIdAsync(string userId, string creditCardId);
    }
}
