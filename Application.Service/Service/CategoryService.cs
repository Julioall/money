using Application.Domain.Interface;
using Application.Domain.Model.Finance;

namespace Application.Service.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public Task AddAsync(Category category)
        {
            return _categoryRepository.AddAsync(category);
        }

        public Task DeleteAsync(string id)
        {
            return _categoryRepository.DeleteAsync(id);
        }

        public Task UpdateAsync(Category category)
        {
            return _categoryRepository.UpdateAsync(category);
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public Task<Category> GetByIdAsync(string id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }
    }
}
