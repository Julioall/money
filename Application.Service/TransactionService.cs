using Application.Domain.Interfaces;
using Application.Domain.Models;

namespace Application.Service
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction>AddAsync(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task<bool> DeleteAsync(string userId, string transactionId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentNullException(nameof(transactionId), "Transaction ID cannot be null or empty.");

            await _transactionRepository.DeleteAsync(userId, transactionId);
            return true;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            await _transactionRepository.UpdateAsync(transaction);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            return await _transactionRepository.GetAllAsync(userId);
        }

        public async Task<Transaction> GetByIdAsync(string userId, string transactionId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentNullException(nameof(transactionId), "Transaction ID cannot be null or empty.");

            return await _transactionRepository.GetByIdAsync(userId, transactionId);
        }
    }
}
