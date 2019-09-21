using System.Linq;
using Microsoft.AspNetCore.SignalR;
using NotadogApi.Domain.Game;
using NotadogApi.Structures;

namespace NotadogApi.Hubs
{
    public class GameHubBroadcast
    {
        private readonly IRoomStorage _roomStorage;
        private readonly IHubContext<GameHub> _hubContext;

        public GameHubBroadcast(IRoomStorage roomStorage, IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
            _roomStorage = roomStorage;
        }

        public void AssignEventHadlers()
        {
            _roomStorage.Changed += HandleRoomStorageChanged;
        }

        private async void HandleRoomStorageChanged(object sender, RoomChangedEventArgs e)
        {
            var room = e.room;
            var players = room.Players;
            var playerIds = players.Select(p => p.Id.ToString()).ToList();

            await _hubContext.Clients.Users(playerIds).SendAsync("OnRoomUpdate", new RoomPayload(room));
        }
    }
}