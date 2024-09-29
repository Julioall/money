using Application.Domain.Model;

namespace Application.Domain.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(string userId, Category category);
        Task<bool> DeleteAsync(string userId, string categoryId);
        Task<Category> UpdateAsync(string userId, Category category);
        Task<IEnumerable<Category>> GetAllAsync(string userId);
        Task<Category> GetByIdAsync(string userId, string categoryId);
    }
}
