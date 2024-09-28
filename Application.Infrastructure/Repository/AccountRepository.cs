using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;

namespace Application.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDocumentStore _documentStore;

        public AccountRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }
        public async Task AddAsync(string userId, Account account)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    user.Finance.Accounts.Add(account);
                    await session.StoreAsync(user);
                    await session.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(string userId, Account account)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var existingAccount = user.Finance.Accounts.FirstOrDefault(a => a.Id == account.Id);
                    if (existingAccount != null)
                    {
                        existingAccount.AccountName = account.AccountName;
                        existingAccount.Balance = account.Balance;
                        existingAccount.HideBalance = account.HideBalance;
                        existingAccount.IconUrl = account.IconUrl;

                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task DeleteAsync(string userId, string accountId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var accountToRemove = user.Finance.Accounts.FirstOrDefault(a => a.Id == accountId);
                    if (accountToRemove != null)
                    {
                        user.Finance.Accounts.Remove(accountToRemove);
                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<IEnumerable<Account>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance.Accounts ?? new List<Account>();
            }
        }

        public async Task<Account> GetByIdAsync(string userId, string accountsId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance?.Accounts.FirstOrDefault(c => c.Id == accountsId);
            }
        }
    }
}
