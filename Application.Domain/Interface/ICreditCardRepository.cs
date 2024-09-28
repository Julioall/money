using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface ICreditCardRepository
    {
        Task AddAsync(string userId, CreditCard creditCard);
        Task DeleteAsync(string userId, string creditCardId);
        Task UpdateAsync(string userId, CreditCard creditCard);
        Task<IEnumerable<CreditCard>> GetAllAsync(string userId);
        Task<CreditCard> GetByIdAsync(string userId, string creditCardId);
    }
}
