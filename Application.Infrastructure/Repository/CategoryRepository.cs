using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;

namespace Application.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDocumentStore _documentStore;

        public CategoryRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task AddAsync(string userId, Category category)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user?.Finance != null)
                {
                    user.Finance.Categories.Add(category);
                    await session.StoreAsync(user);
                    await session.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(string userId, string categoryId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user?.Finance != null)
                {
                    var category = user.Finance.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (category != null)
                    {
                        user.Finance.Categories.Remove(category);
                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance?.Categories ?? new List<Category>();
            }
        }

        public async Task<Category> GetByIdAsync(string userId, string categoryId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance?.Categories.FirstOrDefault(c => c.Id == categoryId);
            }
        }

        public async Task UpdateAsync(string userId, Category updatedCategory)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user?.Finance != null)
                {
                    var existingCategory = user.Finance.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);
                    if (existingCategory != null)
                    {
                        existingCategory.Name = updatedCategory.Name;
                        existingCategory.Description = updatedCategory.Description;
                        existingCategory.Type = updatedCategory.Type;
                        existingCategory.IconUrl = updatedCategory.IconUrl;

                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
