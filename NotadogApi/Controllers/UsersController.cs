using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users.
        /// </summary>  
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _userService.ListAsync();
            return users;
        }
    }
}