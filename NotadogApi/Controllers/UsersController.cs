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
        private readonly UserSignupDtoValidatorAsync _userSignupDtoValidatorAsync;

        public UsersController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator, UserSignupDtoValidatorAsync userSignupDtoValidatorAsync)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userSignupDtoValidatorAsync = userSignupDtoValidatorAsync;
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
        public async Task<IActionResult> Signup(UserSignupDto dto)
        {
            await _userSignupDtoValidatorAsync.ValidateAndThrowAsync(dto);

            var user = await _userService.CreateAsync(dto.Name, dto.Email, dto.Password);
            var token = await _jwtTokenGenerator.CreateToken(user.Id);
            return Ok(token);
        }
    }

}