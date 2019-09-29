using System.Linq;
using Microsoft.AspNetCore.SignalR;
using NotadogApi.Domain.Game;
using NotadogApi.Models;

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

            await _hubContext.Clients
                .Users(players.Select(p => p.Id))
                .SendAsync(GameHubMethod.OnRoomUpdate, new RoomPayload(room));
        }
    }
}