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
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRoomStorage _roomStorage;
        public GameController(ICurrentUserAccessor currentUserAccessor, IRoomStorage roomStorage)
        {
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
            var room = await _roomStorage.CreateRoom(user);

            return Ok(new RoomPayload(room));
        }

        /// <summary>
        /// Connect to public game.
        /// </summary>  
        [HttpPut("public")]
        public async Task<IActionResult> PutPublicAsync(UpdatePublicRoomRequestPayload payload)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.JoinAvailableRoom(user, payload.PlayersMaxCount);

            return Ok(new RoomPayload(room));
        }

        /// <summary>
        /// Connect to private game.
        /// </summary>  
        [HttpPut("private")]
        public async Task<IActionResult> PutPrivateAsync(UpdatePrivateRoomRequestPayload payload)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByPayload(payload.RoomId);
            var existedRoom = await _roomStorage.JoinRoom(user, room, payload.ForceAdding);

            return Ok(new RoomPayload(existedRoom));
        }
    }

}