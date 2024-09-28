using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface IAccountRepository
    {
        Task AddAsync(string userId, Account account);
        Task DeleteAsync(string userId, string accountId);
        Task UpdateAsync(string userId, Account account);
        Task<IEnumerable<Account>> GetAllAsync(string userId);
        Task<Account> GetByIdAsync(string userId, string accountId);
    }
}