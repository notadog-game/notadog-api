using System;

namespace NotadogApi.Domain.Game.States
{
    public abstract class BaseRoomState
    {
        protected readonly Room _room;
        protected readonly DateTime _timestamp;
        public BaseRoomState(Room room)
        {
            _room = room;
            _timestamp = DateTime.Now;
        }

        public DateTime getTimestamp() => _timestamp;
    }
}
