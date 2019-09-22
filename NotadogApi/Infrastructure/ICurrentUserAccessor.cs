using System.Threading.Tasks;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentId();
        Task<User> GetCurrentUserAsync();
    }
}