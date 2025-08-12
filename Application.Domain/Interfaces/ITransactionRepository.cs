using Application.Domain.Models;

namespace Application.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<bool> DeleteAsync(string userId, string transactionId);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllAsync(string userId);
        Task<Transaction> GetByIdAsync(string userId, string transactionId);
    }
}
