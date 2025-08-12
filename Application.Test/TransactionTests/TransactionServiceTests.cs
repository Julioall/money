using Application.Domain.Models;
using Application.Domain.Models.Enums;
using Application.Service;
using Application.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Test.TransactionTests
{
    public class TransactionServiceTests : BaseTest
    {
        private readonly TransactionService _transactionService;
        private readonly UserService _userService;

        public TransactionServiceTests()
        {
            _transactionService = _serviceProvider.GetRequiredService<TransactionService>();
            _userService = _serviceProvider.GetRequiredService<UserService>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTransaction()
        {
            const string userId = "users-200-A";
            var user = CreateUser(userId, "user@example.com");
            await _userService.AddAsync(user);

            var transaction = new Transaction
            {
                Id = "transactions-200-A",
                UserId = userId,
                Amount = 100m,
                CategoryId = "categories-1",
                Date = DateTime.UtcNow,
                Type = TransactionType.Income,
                AccountDestinationId = "accounts-1"
            };

            await _transactionService.AddAsync(transaction);

            var result = await _transactionService.GetByIdAsync(userId, transaction.Id);

            Assert.NotNull(result);
            Assert.Equal(transaction.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenUserIdIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _transactionService.GetByIdAsync(null!, "transactions-1");
            });
        }

        private User CreateUser(string id, string email)
        {
            return new User
            {
                Id = id,
                Account = new UserAccount
                {
                    Email = email,
                    Password = "Securep@ssword123",
                    Role = "User",
                },
                Profile = new UserProfile
                {
                    Name = "Test User",
                },
                Finance = new UserFinance()
            };
        }
    }
}

