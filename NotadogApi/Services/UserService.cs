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

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }
    }
}