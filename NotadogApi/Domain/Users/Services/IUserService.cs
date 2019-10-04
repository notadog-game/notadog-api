using System.Collections.Generic;
using System.Threading.Tasks;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Users.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(string name, string email, string password);
        Task<List<User>> GetAllAsync();
        Task<User> GetOneAsync(int id);
        Task<User> GetOneByEmailAsync(string email);
        Task UpdateOneAsync(int id, UserUpdateDto user);
    }
}
