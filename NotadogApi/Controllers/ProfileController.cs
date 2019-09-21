using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NotadogApi.Security;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Infrastructure;


namespace NotadogApi.Controllers
{
    /// <summary>
	/// Profile API
	/// </summary>
    [Route("/api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public ProfileController(IUserService userService, ICurrentUserAccessor currentUserAccessor)
        {
            _userService = userService;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <summary>
        /// Get Profile.
        /// </summary>  
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _userService.GetOneAsync(id);

            return Ok(user);
        }

        /// <summary>
        /// Update Profile.
        /// </summary>  
        [HttpPut]
        public async Task<IActionResult> PutAsync(UserUpdatePayload user)
        {
            var id = _currentUserAccessor.GetCurrentId();
            await _userService.UpdateOneAsync(id, user);

            return Ok();
        }
    }

}