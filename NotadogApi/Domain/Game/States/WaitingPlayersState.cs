namespace NotadogApi.Domain.Game.States
{
    public class WaitingPlayersState : BaseRoomState
    {
        public WaitingPlayersState(Room room) : base(room)
        {
            StateCode = nameof(WaitingPlayersState);
        }
    }
}

