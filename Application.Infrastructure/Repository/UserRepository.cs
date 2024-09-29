using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;

namespace Application.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDocumentStore _documentStore;

        public UserRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task<User> AddAsync(User user)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
                return user;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(id);
                if (user == null)
                    return false;

                session.Delete(user);
                await session.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<User>().ToListAsync();
            }
        }

        public async Task<User> GetByIdAsync(string id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<User>(id);
            }
        }

        public async Task<User> GetByRoleAsync(string role)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<User>()
                                    .FirstOrDefaultAsync(u => u.Account.Role == role);
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<User>()
                                    .FirstOrDefaultAsync(u => u.Account.Email == email);
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var existingUser = await session.LoadAsync<User>(user.Id);
                if (existingUser == null)
                    throw new InvalidOperationException("User not found.");

                existingUser.Account = user.Account;
                existingUser.Profile = user.Profile;
                existingUser.Finance = user.Finance;

                await session.SaveChangesAsync();
                return existingUser;
            }
        }
    }
}
