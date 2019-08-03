using System.Collections.Generic;
using System.Threading.Tasks;
using NotadogApi.Domain.Models;

namespace NotadogApi.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(string name, string email, string password);
        Task<List<User>> GetAllAsync();
        Task<User> GetOneAsync(int id);
        Task<User> GetOneByEmailAsync(string email);
    }
}
