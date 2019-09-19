using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using NotadogApi.Domain.Game;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Infrastructure;
using NotadogApi.Structures;


namespace NotadogApi.Hubs
{
    struct PlayerPayload
    {
        public int Id;
        public string Email;
        public string Name;
        public int Score;

        public PlayerPayload(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Score = user.Score;
        }
    }

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

        public async Task StartGame()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(id);

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", new RoomPayload(room));
                return;
            }

            room.start(user);
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
        }

        public async Task LeaveRoom()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(id);

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            await _roomStorage.LeaveRoom(user);
            await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
        }

        public async Task Refresh()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var room = await _roomStorage.GetRoomByUserId(id);

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            await Clients.User($"{id}").SendAsync("OnRoomUpdate", new RoomPayload(room));
        }

        public async Task Replay()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var room = await _roomStorage.GetRoomByUserId(id);
            var user = await _currentUserAccessor.GetCurrentUserAsync();

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            room.replay(user);

            await Clients.User($"{id}").SendAsync("OnRoomUpdate", new RoomPayload(room));
        }

        public override async Task OnConnectedAsync()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(id);

            await Clients.User($"{id}").SendAsync("OnConnect", new PlayerPayload(user));

            if (room == null)
            {
                await Clients.User($"{id}").SendAsync("OnRoomUpdate", null);
                return;
            }

            await Clients.User($"{id}").SendAsync("OnRoomUpdate", new RoomPayload(room));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            await Clients.User($"{user.Id}").SendAsync("OnDisconnect", new PlayerPayload(user));
        }
    }
}