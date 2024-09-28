using Application.Domain.Interface;
using Application.Domain.Model;

namespace Application.Service
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(string userId, string transactionId)
        {
            await _transactionRepository.DeleteAsync(userId, transactionId);
            return true;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(string userId)
        {
            return await _transactionRepository.GetAllAsync(userId);
        }

        public async Task<Transaction> GetTransactionByIdAsync(string userId, string transactionId)
        {
            return await _transactionRepository.GetByIdAsync(userId, transactionId);
        }
    }
}
