using System.Collections.Generic;
using System.Threading.Tasks;
using NotadogApi.Domain.Models;
using NotadogApi.Domain.Services;
using NotadogApi.Domain.Repositories;

namespace NotadogApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<List<User>> GetAllAsync() => await _userRepository.GetAllAsync();
        public async Task<User> GetOneAsync(int id) => await _userRepository.GetOneAsync(id);
        public async Task<User> GetOneByEmailAsync(string email) => await _userRepository.GetOneByEmailAsync(email);
    }
}
