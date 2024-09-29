using Application.Domain.Model;

namespace Application.Domain.Interface
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