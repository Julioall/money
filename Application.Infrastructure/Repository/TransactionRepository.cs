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
            _documentStore = documentStore;
        }

        public async Task AddAsync(Transaction transaction)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(transaction.UserId);
                user.Finance.TransactionsIds.Add(transaction.Id); 
                await session.StoreAsync(transaction);
                await session.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string userId, string transactionId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var transaction = await session.LoadAsync<Transaction>(transactionId);
                    if (transaction != null)
                    {
                        user.Finance.TransactionsIds.Remove(transactionId);
                        session.Delete(transaction);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(transaction);
                await session.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var transactions = await session.Query<Transaction>()
                        .Where(t => t.UserId == userId)
                        .ToListAsync();

                    return transactions;
                }

                return Enumerable.Empty<Transaction>();
            }
        }

        public async Task<Transaction> GetByIdAsync(string userId, string transactionId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    return await session.LoadAsync<Transaction>(transactionId);
                }

                return null;
            }
        }
    }
}
