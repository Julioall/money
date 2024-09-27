using Application.Domain.Model.Finance;

namespace Application.Domain.Interface
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);

        Task DeleteAsync(string id);

        Task UpdateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(string id);
    }
}
