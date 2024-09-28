using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);

        Task DeleteAsync(string userId, string transactionId);

        Task UpdateAsync(Transaction transaction);

        Task<IEnumerable<Transaction>> GetAllAsync(string userId);

        Task<Transaction> GetByIdAsync(string userId, string transactionId);
    }
}
