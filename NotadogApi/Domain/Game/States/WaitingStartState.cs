using System;
using System.Threading.Tasks;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class WaitingStartState : BaseRoomState, IRoomState
    {
        public WaitingStartState(Room room) : base(room)
        {
            // TODO: Implement random delay 3-10 sec
            Task.Delay(3000).ContinueWith(_ =>
            {
                _room.changeState(new PlayingState(_room));
            });
        }

        public string getStateCode()
        {
            return nameof(WaitingStartState);
        }

        public void handleUserNotADogAction(User user) { }
    }
}

