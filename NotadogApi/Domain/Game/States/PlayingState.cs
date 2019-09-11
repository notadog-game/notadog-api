using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class PlayingState : BaseRoomState, IRoomState
    {
        private readonly CancellationTokenSource _source;

        public PlayingState(Room room) : base(room)
        {

            _source = new CancellationTokenSource();
            Task.Delay(60000, _source.Token).ContinueWith(_ =>
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
            if (_room.getPlayersCount() - _room.MakedMovePlayers.Count == 1)
            {
                _source.Cancel();
                _room.changeState(new EndState(_room));
            }
        }
    }
}

