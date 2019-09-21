namespace NotadogApi.Domain.Game.States
{
    public class EndState : BaseRoomState
    {
        public EndState(Room room) : base(room)
        {
            StateCode = nameof(EndState);
        }
        
    }
}

