using Application.Domain.Interface;
using Application.Domain.Model;

namespace Application.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }
        public Task AddAsync(string userId, Category category)
        {
            return _categoryRepository.AddAsync(userId, category);
        }

        public Task DeleteAsync(string userId, string id)
        {
            return _categoryRepository.DeleteAsync(userId, id);
        }

        public Task UpdateAsync(string userId, Category category)
        {
            return _categoryRepository.UpdateAsync(userId, category);
        }

        public Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            return _categoryRepository.GetAllAsync(userId);
        }

        public Task<Category> GetByIdAsync(string userId, string id)
        {
            return _categoryRepository.GetByIdAsync(userId, id);
        }
    }
}
