using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using NotadogApi.Domain.Game;
using NotadogApi.Infrastructure;
using NotadogApi.Structures;

namespace NotadogApi.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRoomStorage _roomStorage;

        public GameHub(ICurrentUserAccessor currentUserAccessor, IRoomStorage roomStorage)
        {
            _currentUserAccessor = currentUserAccessor;
            _roomStorage = roomStorage;
        }

        public async Task MakeMove()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(id);

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            room.handleUserNotADogAction(user);
            await Clients.Users(room.getPlayers().Select(u => $"{u.Id}").ToList()).SendAsync("OnMakedMove");
        }

        public override async Task OnConnectedAsync()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var room = await _roomStorage.GetRoomByUserId(id);

            await Clients.All.SendAsync("OnConnect", $"{id}");

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            await Clients.User($"{id}").SendAsync("OnRoomUpdate", new RoomPayload(room));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var id = _currentUserAccessor.GetCurrentId();
            await Clients.All.SendAsync("OnDisconnect", $"{id}");
        }
    }
}