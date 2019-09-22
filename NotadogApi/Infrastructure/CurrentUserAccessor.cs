using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public string GetCurrentId()
        {
            return _httpContextAccessor
                .HttpContext
                .User?
                .Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                .Value;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return await _userService.GetOneAsync(this.GetCurrentId());
        }
    }
}