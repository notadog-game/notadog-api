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
        public IEnumerable<User> Players;
        public string StateCode;

        public RoomPayload(Room room)
        {
            Guid = room.Guid;
            PlayersMaxCount = room.PlayersMaxCount;
            Players = room.Players;
            StateCode = room.getStateCode();
        }
    }
}

