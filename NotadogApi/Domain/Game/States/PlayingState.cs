using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class PlayingState : BaseRoomState
    {
        public PlayingState(Room room) : base(room)
        {
            StateCode = nameof(PlayingState);
        }
        
        public override void HandleUserNotADogAction(User user)
        {
            if (_room.Players.Count - _room.MakedMovePlayerIds.Count < 2)
                _room.ChangeState(new EndState(_room));
            
        }
    }
}

