using System;
using System.Collections.Generic;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game;

namespace NotadogApi.Models
{
    class RoomPayload
    {
        public Guid Guid;
        public int? PlayersMaxCount;
        public int RootId;
        public IEnumerable<User> Players;
        public IEnumerable<int> MakedMovePlayerIds;
        public string StateCode;

        public RoomPayload(Room room)
        {
            Guid = room.Guid;
            PlayersMaxCount = room.PlayersMaxCount;
            RootId = room.RootId;
            Players = room.Players;
            MakedMovePlayerIds = room.MakedMovePlayerIds;
            StateCode = room.GetStateCode();
        }
    }
}

