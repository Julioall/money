using Application.Domain.Models;

namespace Application.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> GetByRoleAsync(string role);
        Task<User> GetByEmailAsync(string username);
        Task<User> UpdateAsync(User user);
    }
}
