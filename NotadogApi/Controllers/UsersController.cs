using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using NotadogApi.Models;
using NotadogApi.Security;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Domain.Exceptions;

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
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            UserLoginDtoValidator userLoginDtoValidator = new UserLoginDtoValidator();
            userLoginDtoValidator.ValidateAndThrow(dto);

            var user = await _userService.GetOneByEmailAsync(dto.Email);

            if (user == null || user.Password != dto.Password)
                return NotFound(new CommonError(ErrorCode.UserNotFound).ToJson());

            var token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(token);
        }

        /// <summary>
        /// Get authentification t.
        /// </summary>  
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupDto credentials)
        {
            var trimmedEmail = credentials.Email.Trim();
            var trimmedPassword = credentials.Password.Trim();
            var user = await _userService.CreateAsync(credentials.Name, trimmedEmail, trimmedPassword);
            var token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(token);
        }
    }

}