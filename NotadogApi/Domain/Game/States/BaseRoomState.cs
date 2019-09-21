using System;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public abstract class BaseRoomState : IRoomState
    {
        protected readonly Room _room;
        public DateTime Timestamp { get; }
        public string StateCode { get; protected set; }

        protected BaseRoomState(Room room)
        {
            _room = room;
            Timestamp = DateTime.Now;
        }

        public virtual void HandleUserNotADogAction(User user) { }
    }
}
