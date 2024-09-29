using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;

namespace Application.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDocumentStore _documentStore;

        public TransactionRepository(IDocumentStore documentStore)
        {
             _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(transaction.UserId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                user.Finance.TransactionsIds.Add(transaction.Id);
                await session.StoreAsync(transaction);
                await session.SaveChangesAsync();
                return transaction;
            }
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var existingTransaction = await session.LoadAsync<Transaction>(transaction.Id);
                if (existingTransaction == null)
                    throw new InvalidOperationException("Transaction not found.");

                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Date = transaction.Date;

                await session.SaveChangesAsync();
                return existingTransaction;
            }
        }


        public async Task<bool> DeleteAsync(string userId, string transactionId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    return false;
                
                var transaction = await session.LoadAsync<Transaction>(transactionId);
                if (transaction == null)
                    return false;
                
                user.Finance.TransactionsIds.Remove(transactionId);
                session.Delete(transaction);
                await session.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                var transactions = await session.Query<Transaction>()
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                return transactions;
            }
        }

        public async Task<Transaction> GetByIdAsync(string transactionId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Transaction>(transactionId);
            }
        }
    }
}