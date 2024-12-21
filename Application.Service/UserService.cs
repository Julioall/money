using Application.Domain.Interfaces;
using Application.Domain.Models;
using FluentValidation;

namespace Application.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        }

        public async Task<User> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return await _userRepository.AddAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "User ID cannot be null or empty.");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "User ID cannot be null or empty.");

            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");

            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByRoleAsync(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty.");

            return await _userRepository.GetByRoleAsync(role);
        }
    }
}
