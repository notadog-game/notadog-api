using System.Collections.Generic;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class PlayingState : BaseRoomState, IRoomState
    {
        private readonly List<User> _makedMovePlayers;

        public PlayingState(Room room) : base(room)
        {
            _makedMovePlayers = new List<User>();

            Task.Delay(3000).ContinueWith(_ =>
            {
                _room.changeState(new EndState(_room));
            });
        }

        public string getStateCode()
        {
            return nameof(PlayingState);
        }

        public void handleUserNotADogAction(User user)
        {
            _makedMovePlayers.Add(user);
            // if (_room.getPlayersCount() - _makedMovePlayers.Count == 1) _room.changeState(new EndState(_room));
        }
    }
}

