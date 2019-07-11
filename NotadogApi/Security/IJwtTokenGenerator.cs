using System.Threading.Tasks;

namespace NotadogApi.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(int id);
    }
}