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
             _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task<Account> AddAsync(string userId, Account account)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                user.Finance.Accounts.Add(account);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
                return account;
            }
        }

        public async Task<Account> UpdateAsync(string userId, Account account)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                var existingAccount = user.Finance.Accounts.FirstOrDefault(a => a.Id == account.Id);
                if (existingAccount == null)
                    throw new InvalidOperationException("Account not found.");

                existingAccount.AccountName = account.AccountName;
                existingAccount.Balance = account.Balance;
                existingAccount.HideBalance = account.HideBalance;
                existingAccount.IconUrl = account.IconUrl;

                await session.SaveChangesAsync();
                return existingAccount;
            }
        }


        public async Task<bool> DeleteAsync(string userId, string accountId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    return false;

                var accountToRemove = user.Finance.Accounts.FirstOrDefault(a => a.Id == accountId);
                if (accountToRemove == null)
                    return false; 

                user.Finance.Accounts.Remove(accountToRemove);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();

                return true;
            }
        }


        public async Task<IEnumerable<Account>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance.Accounts ?? new List<Account>();
            }
        }

        public async Task<Account> GetByIdAsync(string userId, string accountsId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance?.Accounts.FirstOrDefault(c => c.Id == accountsId);
            }
        }
    }
}
