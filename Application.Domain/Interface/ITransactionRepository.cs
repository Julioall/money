using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<bool> DeleteAsync(string userId, string transactionId);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllAsync(string userId);
        Task<Transaction> GetByIdAsync(string transactionId);
    }
}
