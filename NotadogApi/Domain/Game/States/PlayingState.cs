using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class PlayingState : BaseRoomState, IRoomState
    {
        public PlayingState(Room room) : base(room) { }

        public string getStateCode()
        {
            return nameof(PlayingState);
        }

        public void handleUserNotADogAction(User user)
        {
            if (_room.Players.Count - _room.MakedMovePlayerIds.Count < 2)
            {
                _room.changeState(new EndState(_room));
            }
        }
    }
}

