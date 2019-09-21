using System;
using System.Threading.Tasks;

namespace NotadogApi.Domain.Game.States
{
    public class WaitingStartState : BaseRoomState
    {
        public WaitingStartState(Room room) : base(room)
        {
            StateCode = nameof(WaitingStartState);
            var ms = new Random().Next(3000, 10000);

            Task.Delay(ms).ContinueWith(_ =>
            {
                _room.ChangeState(new PlayingState(_room));
            });
        }
    }
}

