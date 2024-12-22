using Application.Domain.Models;

namespace Application.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(string userId, Account account);
        Task<bool> DeleteAsync(string userId, string accountId);
        Task<Account> UpdateAsync(string userId, Account account);
        Task<IEnumerable<Account>> GetAllAsync(string userId);
        Task<Account> GetByIdAsync(string userId, string accountId);
    }
}