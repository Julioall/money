using Application.Domain.Interface;
using Application.Domain.Model;

namespace Application.Service
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> CreateAccountAsync(string userId, Account account)
        {
            await _accountRepository.AddAsync(userId, account);
            return account;
        }

        public async Task<bool> DeleteAccountAsync(string userId, string accountId)
        {
            await _accountRepository.DeleteAsync(userId, accountId);
            return true;
        }

        public async Task<Account> UpdateAccountAsync(string userId, Account account)
        {
            await _accountRepository.UpdateAsync(userId, account);
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync(string userId)
        {
            return await _accountRepository.GetAllAsync(userId);
        }

        public async Task<Account> GetAccountByIdAsync(string userId, string accountId)
        {
            return await _accountRepository.GetByIdAsync(userId, accountId);
        }
    }
}
