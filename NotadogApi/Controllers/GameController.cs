using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NotadogApi.Security;
using NotadogApi.Structures;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Infrastructure;
using NotadogApi.Domain.Game;
using NotadogApi.Models;

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
        public async Task<IActionResult> PostAsync(CreatePrivateRoomRequestPayload payload)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.CreatePrivateRoom(user, payload.ForceAdding);

            return Ok(new RoomPayload(room));
        }

        /// <summary>
        /// Connect to public game.
        /// </summary>  
        [HttpPut("public")]
        public async Task<IActionResult> PutPublicAsync(UpdatePublicRoomRequestPayload payload)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.AddUserToAvailableRoom(user, payload.ForceAdding);

            return Ok(new RoomPayload(room));
        }

        /// <summary>
        /// Connect to private game.
        /// </summary>  
        [HttpPut("private")]
        public async Task<IActionResult> PutPrivateAsync(UpdatePrivateRoomRequestPayload payload)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetPrivateRoomById(payload.RoomId);
            var existedRoom = await _roomStorage.AddUserToRoom(user, room, payload.ForceAdding);

            return Ok(new RoomPayload(existedRoom));
        }
    }

}