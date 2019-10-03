using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotadogApi.Domain.Game;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Infrastructure;
using NotadogApi.Models;


namespace NotadogApi.Hubs
{
    public class PlayerPayload
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
    public class GameHubBase : Hub
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRoomStorage _roomStorage;

        public GameHubBase(ICurrentUserAccessor currentUserAccessor, IRoomStorage roomStorage)
        {
            _currentUserAccessor = currentUserAccessor;
            _roomStorage = roomStorage;
        }

        public virtual async Task StartGame()
        {
            var (user, room) = await GetContext();

            if (room == null)
            {
                await SendRoomDtoAsync(null, user);
                return;
            }

            room.Start(user);
        }

        public virtual async Task MakeMove()
        {
            var (user, room) = await GetContext();

            if (room == null)
            {
                await SendRoomDtoAsync(null, user);
                return;
            }

            room.HandleUserNotADogAction(user);
        }

        public virtual async Task LeaveRoom()
        {
            var (user, _) = await GetContext();
            await _roomStorage.LeaveRoom(user);
            await SendRoomDtoAsync(null, user);
        }

        public virtual async Task Refresh()
        {
            var (user, room) = await GetContext();
            await SendRoomDtoAsync(room, user);
        }

        public virtual async Task Replay()
        {
            var (user, room) = await GetContext();
            room?.Replay(user);
            await SendRoomDtoAsync(room, user);
        }

        public override async Task OnConnectedAsync()
        {
            var (user, room) = await GetContext();
            await Clients.User(user.Id)
                .SendAsync(GameHubMethod.OnConnect, new PlayerPayload(user));
            await SendRoomDtoAsync(room, user);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var (user, _) = await GetContext();
            await Clients.User(user.Id)
                .SendAsync(GameHubMethod.OnDisconnect, new PlayerPayload(user));
        }

        private Task SendRoomDtoAsync(Room room, User user) => Clients.User(user.Id)
            .SendAsync(GameHubMethod.OnRoomUpdate, room != null ? new RoomDto(room) : null);

        private async Task<(User user, Room room)> GetContext()
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync();
            var room = await _roomStorage.GetRoomByUserId(user.Id);
            return (user, room);
        }
    }
}