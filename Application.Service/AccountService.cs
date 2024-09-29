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

        public async Task<Account> AddAsync(string userId, Account account)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (account == null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");

            await _accountRepository.AddAsync(userId, account);
            return account;
        }

        public async Task<bool> DeleteAsync(string userId, string accountId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrEmpty(accountId))
                throw new ArgumentNullException(nameof(accountId), "Account ID cannot be null or empty.");

            return await _accountRepository.DeleteAsync(userId, accountId);
        }

        public async Task<Account> UpdateAsync(string userId, Account account)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (account == null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");

            await _accountRepository.UpdateAsync(userId, account);
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            return await _accountRepository.GetAllAsync(userId);
        }

        public async Task<Account> GetByIdAsync(string userId, string accountId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrEmpty(accountId))
                throw new ArgumentNullException(nameof(accountId), "Account ID cannot be null or empty.");

            return await _accountRepository.GetByIdAsync(userId, accountId);
        }
    }
}
