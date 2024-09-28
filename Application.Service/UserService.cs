using Application.Domain.Interface;
using Application.Domain.Model;

namespace Application.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task AddAsync(User user)
        {
            return _userRepository.AddAsync(user);
        }

        public Task DeleteAsync(string id)
        {
            return _userRepository.DeleteAsync(id);
        }

        public Task UpdateAsync(User user)
        {
            return _userRepository.UpdateAsync(user);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public Task<User> GetByIdAsync(string id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return _userRepository.GetByEmailAsync(email);
        }

        public Task<User> GetByRoleAsync(string role)
        {
            return _userRepository.GetByRoleAsync(role);
        }
    }
}
