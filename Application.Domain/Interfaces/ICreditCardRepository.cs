using Application.Domain.Models;

namespace Application.Domain.Interfaces
{
    public interface ICreditCardsRepository
    {
        Task<CreditCard> AddAsync(string userId, CreditCard CreditCards);
        Task<bool> DeleteAsync(string userId, string CreditCardsId);
        Task<CreditCard> UpdateAsync(string userId, CreditCard CreditCards);
        Task<IEnumerable<CreditCard>> GetAllAsync(string userId);
        Task<CreditCard> GetByIdAsync(string userId, string CreditCardsId);
    }
}
