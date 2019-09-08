using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        int GetCurrentId();
        Task<User> GetCurrentUserAsync();
    }
}