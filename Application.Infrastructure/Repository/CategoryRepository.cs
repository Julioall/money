using Application.Domain.Interface;
using Application.Domain.Model.Finance;
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

        public async Task AddAsync(Category category)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(category);
                await session.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var category = await session.LoadAsync<Category>(id.ToString());
                if (category != null)
                {
                    session.Delete(category);
                    await session.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Category>().ToListAsync();
            }
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Category>(id.ToString());
            }
        }

        public async Task UpdateAsync(Category category)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(category);
                await session.SaveChangesAsync();
            }
        }
    }
}
