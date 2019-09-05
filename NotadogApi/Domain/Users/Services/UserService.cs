using System.Collections.Generic;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Domain.Users.Repositories;

namespace NotadogApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<User> CreateAsync(string name, string email, string password) => await _userRepository.CreateAsync(name, email, password);
        public async Task<List<User>> GetAllAsync() => await _userRepository.GetAllAsync();
        public async Task<User> GetOneAsync(int id) => await _userRepository.GetOneAsync(id);
        public async Task<User> GetOneByEmailAsync(string email) => await _userRepository.GetOneByEmailAsync(email);
        public async Task UpdateOneAsync(int id, UserUpdatePayload user)
        {
            var currentUser = await _userRepository.GetOneAsync(id);

            currentUser.Name = user.Name ?? currentUser.Name;
            currentUser.Email = user.Email ?? currentUser.Email;
            currentUser.Password = user.Password ?? currentUser.Password;

            await _userRepository.UpdateOneAsync(currentUser);
        }
    }
}
