using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class EndState : BaseRoomState, IRoomState
    {
        public EndState(Room room) : base(room) { }

        public string getStateCode()
        {
            return nameof(EndState);
        }

        public void handleUserNotADogAction(User user) { }
    }
}

