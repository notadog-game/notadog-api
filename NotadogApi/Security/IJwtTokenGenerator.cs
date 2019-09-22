using System.Threading.Tasks;

namespace NotadogApi.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string id);
    }
}