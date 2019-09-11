using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using NotadogApi.Security;
using NotadogApi.Structures;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Infrastructure;
using NotadogApi.Domain.Game;
using NotadogApi.Hubs;

namespace NotadogApi.Controllers
{
    /// <summary>
	/// Profile API
	/// </summary>
    [Route("/api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRoomStorage _roomStorage;
        public GameController(IUserService userService, ICurrentUserAccessor currentUserAccessor, IRoomStorage roomStorage)
        {
            _userService = userService;
            _currentUserAccessor = currentUserAccessor;
            _roomStorage = roomStorage;
        }

        /// <summary>
        /// Create game.
        /// </summary>  
        [HttpPost]
        public async Task<IActionResult> PostAsync(Boolean forceAdding)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var newRoom = new Room();
            var room = await _roomStorage.AddUserToRoom(user, newRoom, forceAdding);

            if (room == null) return NotFound();
            return Ok(new RoomPayload(room));
        }

        /// <summary>
        /// Connect to game.
        /// </summary>  
        [HttpPut]
        public async Task<IActionResult> PutAsync(Boolean forceAdding)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.AddUserToAvailableRoom(user, forceAdding);

            if (room == null) return NotFound();
            return Ok(new RoomPayload(room));
        }
    }

}