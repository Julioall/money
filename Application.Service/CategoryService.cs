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

        public async Task<Category> AddAsync(string userId, Category category)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            return await _categoryRepository.AddAsync(userId, category);
        }

        public async Task<bool> DeleteAsync(string userId, string categoryId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrEmpty(categoryId))
                throw new ArgumentNullException(nameof(categoryId), "Category ID cannot be null or empty.");

            return await _categoryRepository.DeleteAsync(userId, categoryId);
        }

        public async Task<Category> UpdateAsync(string userId, Category category)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            return await _categoryRepository.UpdateAsync(userId, category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            return await _categoryRepository.GetAllAsync(userId);
        }

        public async Task<Category> GetByIdAsync(string userId, string categoryId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrEmpty(categoryId))
                throw new ArgumentNullException(nameof(categoryId), "Category ID cannot be null or empty.");

            return await _categoryRepository.GetByIdAsync(userId, categoryId);
        }
    }
}
