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
            (User user, Room room) = await getContext();

            if (room == null)
            {
                await SendRoomPayloadAsync(null, user);
                return;
            }

            room.start(user);
        }

        public async Task MakeMove()
        {
            (User user, Room room) = await getContext();

            if (room == null)
            {
                await SendRoomPayloadAsync(null, user);
                return;
            }

            room.handleUserNotADogAction(user);
        }

        public async Task LeaveRoom()
        {
            (User user, Room room) = await getContext();
            await _roomStorage.LeaveRoom(user);
            await SendRoomPayloadAsync(null, user);
        }

        public async Task Refresh()
        {
            (User user, Room room) = await getContext();
            await SendRoomPayloadAsync(room, user);
        }

        public async Task Replay()
        {
            (User user, Room room) = await getContext();
            room?.replay(user);
            await SendRoomPayloadAsync(room, user);
        }

        public override async Task OnConnectedAsync()
        {
            (User user, Room room) = await getContext();
            await Clients.User($"{user.Id}").SendAsync("OnConnect", new PlayerPayload(user));
            await SendRoomPayloadAsync(room, user);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            (User user, Room room) = await getContext();
            await Clients.User($"{user.Id}").SendAsync("OnDisconnect", new PlayerPayload(user));
        }

        private Task SendRoomPayloadAsync(Room room, User user)
        {
            return Clients.User($"{user.Id}").SendAsync("OnRoomUpdate", room != null ? new RoomPayload(room) : null);
        }

        private async Task<(User user, Room room)> getContext()
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(user.Id);
            return (user, room);
        }
    }
}