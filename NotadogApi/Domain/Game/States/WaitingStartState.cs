using System;
using System.Threading.Tasks;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class WaitingStartState : BaseRoomState, IRoomState
    {
        public WaitingStartState(Room room) : base(room)
        {
            Random rnd = new Random();
            int ms = rnd.Next(3000, 10000);

            Task.Delay(ms).ContinueWith(_ =>
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

