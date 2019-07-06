using System.Collections.Generic;
using System.Threading.Tasks;
using NotadogApi.Domain.Models;

namespace NotadogApi.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
}