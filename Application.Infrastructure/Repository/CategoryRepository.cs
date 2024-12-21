using Application.Domain.Interfaces;
using Application.Domain.Models;

namespace Application.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IServiceRavenDB _serviceRavenDB;

        public CategoryRepository(IServiceRavenDB serviceRavenDB)
        {
             _serviceRavenDB = serviceRavenDB;
        }

        public async Task<Category> AddAsync(string userId, Category category)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                if (user.Finance.Categories.Any(c => c.Name == category.Name))
                    throw new InvalidOperationException($"Category with the name '{category.Name}' already exists.");

                category.Id = $"category-{Guid.NewGuid()}";


                user.Finance.Categories.Add(category);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();

                return category;
            }
        }


        public async Task<bool> DeleteAsync(string userId, string categoryId)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    return false;

                var categoryToRemove = user.Finance.Categories.FirstOrDefault(a => a.Id == categoryId);
                if (categoryToRemove == null)
                    return false;

                user.Finance.Categories.Remove(categoryToRemove);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
                return true;
            }
        }


        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance?.Categories ?? new List<Category>();
            }
        }

        public async Task<Category> GetByIdAsync(string userId, string categoryId)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance?.Categories.FirstOrDefault(c => c.Id == categoryId);
            }
        }

        public async Task<Category> UpdateAsync(string userId, Category updatedCategory)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                var existingCategory = user.Finance.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);
                if (existingCategory == null)
                    throw new KeyNotFoundException($"Category with ID '{updatedCategory.Id}' not found.");

                existingCategory.Name = updatedCategory.Name;
                existingCategory.Description = updatedCategory.Description;
                existingCategory.Type = updatedCategory.Type;
                existingCategory.Icon = updatedCategory.Icon;
                existingCategory.Background = updatedCategory.Background;

                await session.StoreAsync(user);
                await session.SaveChangesAsync();

                return existingCategory;
            }
        }
    }
}
