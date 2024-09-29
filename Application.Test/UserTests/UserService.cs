using Raven.Client.Documents;
using Application.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using Application.Domain.Model;
using Application.Service;

namespace Application.Test.UserTests
{
    public class UserServiceTests : TestHelper
    {
        private readonly UserService _userService;
        private readonly IDocumentStore _documentStore;

        public UserServiceTests() : base()
        {
            _userService = ServiceProvider.GetRequiredService<UserService>();
            _documentStore = ServiceProvider.GetRequiredService<IDocumentStore>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var user = CreateUser("1", "test@example.com");

            // Act
            await _userService.AddAsync(user);

            // Assert
            var retrievedUser = await GetUserFromDatabase("1");
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Account.Email, retrievedUser.Account.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = CreateUser("2", email);
            await _userService.AddAsync(user);

            // Act
            var result = await _userService.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Account.Email);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = CreateUser("3", "test@example.com");
            await _userService.AddAsync(user);
            user.Profile.Name = "Updated User";

            // Act
            await _userService.UpdateAsync(user);

            // Assert
            var updatedUser = await GetUserFromDatabase("3");
            Assert.Equal("Updated User", updatedUser.Profile.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            // Arrange
            var userId = "4";
            var user = CreateUser(userId, "test@example.com");
            await _userService.AddAsync(user);

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            var deletedUser = await GetUserFromDatabase(userId);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = "5";
            var user = CreateUser(userId, "test@example.com");
            await _userService.AddAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        private User CreateUser(string id, string email)
        {
            return new User
            {
                Id = id,
                Account = new UserAccount
                {
                    Email = email,
                    Password = "securepassword",
                    Role = "User"
                },
                Profile = new UserProfile
                {
                    Name = "Test User"
                },
                Finance = new UserFinance()

            };
        }

        private async Task<User> GetUserFromDatabase(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<User>(userId);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var email = "duplicate@example.com";
            var user1 = CreateUser("6", email);
            var user2 = CreateUser("7", email);
            await _userService.AddAsync(user1);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.AddAsync(user2);
            });
        }

        [Fact]
        public async Task GetByRoleAsync_ShouldReturnUserWithSpecificRole()
        {
            // Arrange
            var user = CreateUser("8", "admin@example.com");
            user.Account.Role = "Admin";
            await _userService.AddAsync(user);

            // Act
            var result = await _userService.GetByRoleAsync("Admin");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.Account.Role);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var user1 = CreateUser("10", "user1@example.com");
            var user2 = CreateUser("11", "user2@example.com");
            await _userService.AddAsync(user1);
            await _userService.AddAsync(user2);

            var result = await _userService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var nonExistentUserId = "999";

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.DeleteAsync(nonExistentUserId);
            });
        }

    }
}
