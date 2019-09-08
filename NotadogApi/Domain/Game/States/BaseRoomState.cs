using System;

namespace NotadogApi.Domain.Game.States
{
    public abstract class BaseRoomState
    {
        protected readonly Room _room;
        public event EventHandler OnStateChaged;
        public BaseRoomState(Room room)
        {
            _room = room;
        }
    }
}
