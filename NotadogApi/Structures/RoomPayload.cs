using System;
using System.Collections.Generic;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game;

namespace NotadogApi.Structures
{
    struct RoomPayload
    {
        public Guid Guid;
        public int PlayersMaxCount;
        public int RootId;
        public IEnumerable<User> Players;
        public IEnumerable<User> MakedMovePlayers;
        public string StateCode;

        public RoomPayload(Room room)
        {
            Guid = room.Guid;
            PlayersMaxCount = room.PlayersMaxCount;
            RootId = room.RootId;
            Players = room.Players;
            MakedMovePlayers = room.MakedMovePlayers;
            StateCode = room.getStateCode();
        }
    }
}

