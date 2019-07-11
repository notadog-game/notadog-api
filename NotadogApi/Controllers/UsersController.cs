using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using NotadogApi.Models;
using NotadogApi.Security;
using NotadogApi.Domain.Models;
using NotadogApi.Domain.Services;

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
        public async Task<IActionResult> Login(UserCredentials credentials)
        {
            var user = await _userService.GetOneByEmailAsync(credentials.Email);

            if (user == null)
            {
                return BadRequest(HttpStatusCode.Forbidden);
            }

            if (user.Password != credentials.Password)
            {
                return BadRequest(HttpStatusCode.Forbidden);
            }

            var Token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(Token);
        }
    }

}