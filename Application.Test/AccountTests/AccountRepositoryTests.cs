using Application.Domain.Interfaces;
using Application.Domain.Models;
using Application.Test.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test.AccountTests
{
    public class AccountRepositoryTests : BaseTest
    {
        private readonly IAccountRepository _accounts;

        public AccountRepositoryTests()
        {
            _accounts = _serviceProvider.GetRequiredService<IAccountRepository>();
        }

        [Fact]
        public async Task AddAsync_ShouldGenerateIdentifier_AndAllowUpdateAndDelete()
        {
            const string userId = "users-account-test";
            await CreateUserAsync(userId);

            var account = new Account
            {
                AccountName = "Conta principal",
                Balance = 150.50m,
                HideBalance = false,
                IconUrl = "img/logo-1.svg",
                BankName = "Banco exemplo",
                AccountType = "checking"
            };

            var created = await _accounts.AddAsync(userId, account);
            Assert.False(string.IsNullOrWhiteSpace(created.Id));

            var loaded = await _accounts.GetByIdAsync(userId, created.Id!);
            Assert.NotNull(loaded);
            Assert.Equal("Banco exemplo", loaded.BankName);

            created.AccountName = "Conta atualizada";
            created.BankName = "Outro banco";
            await _accounts.UpdateAsync(userId, created);

            var updated = await _accounts.GetByIdAsync(userId, created.Id!);
            Assert.Equal("Conta atualizada", updated.AccountName);
            Assert.Equal("Outro banco", updated.BankName);

            Assert.True(await _accounts.DeleteAsync(userId, created.Id!));
            Assert.Empty(await _accounts.GetAllAsync(userId));
        }

        [Fact]
        public async Task GetAllAsync_ShouldRepairMissingAndDuplicateLegacyAccountIds()
        {
            const string userId = "users-legacy-account-test";
            await CreateUserAsync(userId);
            using (var session = _store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                user.Finance.Accounts.Add(new Account
                {
                    AccountName = "Antiga conta",
                    Balance = 0,
                    HideBalance = false,
                    IconUrl = "img/logo-1.svg"
                });
                user.Finance.Accounts.Add(new Account
                {
                    Id = "duplicate",
                    AccountName = "Outra conta",
                    Balance = 10,
                    HideBalance = false,
                    IconUrl = "img/logo-2.svg"
                });
                user.Finance.Accounts.Add(new Account
                {
                    Id = "duplicate",
                    AccountName = "Terceira conta",
                    Balance = 20,
                    HideBalance = false,
                    IconUrl = "img/logo-3.svg"
                });
                await session.SaveChangesAsync();
            }

            var accounts = (await _accounts.GetAllAsync(userId)).ToArray();
            Assert.Equal(3, accounts.Length);
            Assert.All(accounts, a => Assert.False(string.IsNullOrWhiteSpace(a.Id)));
            Assert.Equal(3, accounts.Select(a => a.Id).Distinct().Count());

            using var verifySession = _store.OpenAsyncSession();
            var persisted = await verifySession.LoadAsync<User>(userId);
            Assert.Equal(3, persisted.Finance.Accounts.Select(a => a.Id).Distinct().Count());
        }

        private async Task CreateUserAsync(string id)
        {
            using var session = _store.OpenAsyncSession();
            await session.StoreAsync(new User
            {
                Id = id,
                Account = new UserAccount
                {
                    Email = $"{id}@example.com",
                    Password = "TestOnly123!",
                    Role = "User"
                },
                Profile = new UserProfile { Name = "Teste" },
                Finance = new UserFinance()
            });
            await session.SaveChangesAsync();
        }
    }
}
