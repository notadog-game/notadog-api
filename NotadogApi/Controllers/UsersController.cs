using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NotadogApi.Models;
using NotadogApi.Security;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Users.Services;

namespace NotadogApi.Controllers
{
    /// <summary>
	/// Users API
	/// </summary>
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UsersController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Get all users.
        /// </summary>  
        [HttpGet]
        public async Task<List<User>> GetAllAsync() => await _userService.GetAllAsync();

        /// <summary>
        /// Get authentification token.
        /// </summary>  
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCredentials credentials)
        {
            var user = await _userService.GetOneByEmailAsync(credentials.Email);

            if (user == null)
                return NotFound();

            if (user.Password != credentials.Password)
                return Unauthorized();

            var token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(token);
        }

        /// <summary>
        /// Get authentification t.
        /// </summary>  
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupCredentials credentials)
        {
            var user = await _userService.CreateAsync(credentials.Name, credentials.Email, credentials.Password);
            var token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(token);
        }
    }

}