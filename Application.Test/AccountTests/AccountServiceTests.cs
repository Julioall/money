using Application.Domain.Models;
using Application.Service;
using Application.Test.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test.AccountTests;

public class AccountServiceTests : BaseTest
{
    private const string UserId = "accounts-test-user";
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _accountService = _serviceProvider.GetRequiredService<AccountService>();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistAccountAndAssignId()
    {
        await SeedUserAsync(UserId);
        var account = CreateAccount();

        var createdAccount = await _accountService.AddAsync(UserId, account);
        var savedUser = await GetUserFromDatabase(UserId);

        Assert.StartsWith("account-", createdAccount.Id);
        Assert.Single(savedUser!.Finance.Accounts);
        Assert.Equal(createdAccount.Id, savedUser.Finance.Accounts[0].Id);
        Assert.Equal(account.AccountName, savedUser.Finance.Accounts[0].AccountName);
        Assert.Equal(account.Balance, savedUser.Finance.Accounts[0].Balance);
        Assert.Equal(account.HideBalance, savedUser.Finance.Accounts[0].HideBalance);
        Assert.Equal(account.IconUrl, savedUser.Finance.Accounts[0].IconUrl);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistAllEditableAccountFields()
    {
        var account = CreateAccount();
        account.Id = "account-existing";
        await SeedUserAsync(UserId, account);
        var updatedAccount = CreateAccount(
            name: "Updated account",
            balance: 980.75m,
            hideBalance: true,
            iconUrl: "img/logo-9.svg");
        updatedAccount.Id = account.Id;

        await _accountService.UpdateAsync(UserId, updatedAccount);
        var savedUser = await GetUserFromDatabase(UserId);
        var savedAccount = Assert.Single(savedUser!.Finance.Accounts);

        Assert.Equal("Updated account", savedAccount.AccountName);
        Assert.Equal(980.75m, savedAccount.Balance);
        Assert.True(savedAccount.HideBalance);
        Assert.Equal("img/logo-9.svg", savedAccount.IconUrl);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveExistingAccountAndReturnFalseForMissingAccount()
    {
        var account = CreateAccount();
        account.Id = "account-to-delete";
        await SeedUserAsync(UserId, account);

        var removed = await _accountService.DeleteAsync(UserId, account.Id);
        var missingAccountRemoved = await _accountService.DeleteAsync(UserId, "account-missing");
        var savedUser = await GetUserFromDatabase(UserId);

        Assert.True(removed);
        Assert.False(missingAccountRemoved);
        Assert.Empty(savedUser!.Finance.Accounts);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAccountsForUser()
    {
        var checking = CreateAccount(name: "Checking");
        checking.Id = "account-checking";
        var savings = CreateAccount(name: "Savings", balance: 300m);
        savings.Id = "account-savings";
        await SeedUserAsync(UserId, checking, savings);

        var accounts = await _accountService.GetAllAsync(UserId);

        Assert.Collection(
            accounts,
            item => Assert.Equal("Checking", item.AccountName),
            item => Assert.Equal("Savings", item.AccountName));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccountOrNullWhenMissing()
    {
        var account = CreateAccount();
        account.Id = "account-to-find";
        await SeedUserAsync(UserId, account);

        var result = await _accountService.GetByIdAsync(UserId, account.Id);
        var missingResult = await _accountService.GetByIdAsync(UserId, "account-missing");

        Assert.NotNull(result);
        Assert.Equal(account.AccountName, result.AccountName);
        Assert.Null(missingResult);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowWhenUserDoesNotExist()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _accountService.AddAsync("missing-user", CreateAccount()));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowWhenAccountDoesNotExist()
    {
        await SeedUserAsync(UserId);
        var account = CreateAccount();
        account.Id = "account-missing";

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _accountService.UpdateAsync(UserId, account));
    }

    [Fact]
    public async Task AccountOperations_ShouldRejectBlankUserIds()
    {
        var account = CreateAccount();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.AddAsync(" ", account));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.UpdateAsync("", account));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.DeleteAsync("\t", "account-1"));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.GetAllAsync(" "));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.GetByIdAsync("", "account-1"));
    }

    [Fact]
    public async Task AccountOperations_ShouldRejectMissingAccountIds()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.DeleteAsync(UserId, " "));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.GetByIdAsync(UserId, ""));
    }

    [Fact]
    public async Task AddAndUpdateAsync_ShouldRejectNullAccounts()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.AddAsync(UserId, null!));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _accountService.UpdateAsync(UserId, null!));
    }

    private async Task SeedUserAsync(string userId, params Account[] accounts)
    {
        var user = new User
        {
            Id = userId,
            Account = new UserAccount
            {
                Email = $"{userId}@example.com",
                Password = "test-password-hash",
                Role = "User"
            },
            Profile = new UserProfile { Name = "Account Test User" },
            Finance = new UserFinance { Accounts = accounts.ToList() }
        };

        using var session = _store.OpenAsyncSession();
        await session.StoreAsync(user, userId);
        await session.SaveChangesAsync();
    }

    private async Task<User?> GetUserFromDatabase(string userId)
    {
        using var session = _store.OpenAsyncSession();
        return await session.LoadAsync<User>(userId);
    }

    private static Account CreateAccount(
        string name = "Everyday account",
        decimal balance = 125.50m,
        bool hideBalance = false,
        string iconUrl = "img/logo-1.svg")
    {
        return new Account
        {
            AccountName = name,
            Balance = balance,
            HideBalance = hideBalance,
            IconUrl = iconUrl
        };
    }
}
