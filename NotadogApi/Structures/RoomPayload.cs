using System.Collections.Generic;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game;

namespace NotadogApi.Structures
{
    struct RoomPayload
    {
        public string Guid;
        public string StateCode;
        public int PlayersMaxCount;
        public IEnumerable<User> Players;

        public RoomPayload(Room room)
        {
            Guid = room.getGuid();
            StateCode = room.getStateCode();
            PlayersMaxCount = room.getPlayersMaxCount();
            Players = room.getPlayers();
        }
    }
}

