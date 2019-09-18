using System;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public class WaitingPlayersState : BaseRoomState, IRoomState
    {
        public WaitingPlayersState(Room room) : base(room)
        {
            _room.clearMakedMovePlayers();
        }

        public string getStateCode()
        {
            return nameof(WaitingPlayersState);
        }

        public void handleUserNotADogAction(User user) { }
    }
}

