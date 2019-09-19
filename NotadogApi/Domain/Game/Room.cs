using System;
using System.Collections.Generic;
using System.Linq;

using NotadogApi.Domain.Game.States;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public class RoomChangedEventArgs : EventArgs
    {
        private Room _room;

        public RoomChangedEventArgs(Room room)
        {
            _room = room;
        }
        public Room room
        {
            get { return _room; }
        }
    }

    public class Room
    {
        public Guid Guid { get; }
        public List<User> Players { get; }
        public List<int> MakedMovePlayerIds { get; }
        public int? PlayersMaxCount { get; }
        public int RootId { get; set; }
        private IRoomState _roomState;
        private const int PlayersMinCount = 2;
        public event EventHandler<RoomChangedEventArgs> Changed;

        public Room(int? playersMaxCount = null)
        {
            if (isPublic() && playersMaxCount < PlayersMinCount) throw new Exception("");

            Guid = Guid.NewGuid();
            Players = new List<User>();
            MakedMovePlayerIds = new List<int>();
            PlayersMaxCount = playersMaxCount;

            _roomState = new WaitingPlayersState(this);
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        public string getStateCode() => _roomState.getStateCode();

        public Boolean isPublic() => PlayersMaxCount.HasValue;

        public void changeState(IRoomState state)
        {
            lock (_roomState)
            {
                _roomState = state;
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void addPlayer(User user)
        {
            lock (Players)
            {
                // TODO: Implement exception
                if (_roomState.getStateCode() != nameof(WaitingPlayersState)) throw new Exception("");
                if (Players.Any(player => player.Id == user.Id)) throw new Exception("");
                Players.Add(user);

                if (isPublic() && PlayersMaxCount == Players.Count)
                {
                    changeState(new WaitingStartState(this));
                    return;
                }
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void removePlayer(User user)
        {
            lock (Players)
            {
                var player = Players.FirstOrDefault(p => p.Id == user.Id);
                if (player == null) throw new Exception("");
                Players.Remove(player);
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void start(User user)
        {
            if (RootId != user.Id) throw new Exception("");
            if (_roomState.getStateCode() != nameof(WaitingPlayersState)) throw new Exception("");
            if (Players.Count < PlayersMinCount) throw new Exception("");

            changeState(new WaitingStartState(this));
        }

        public void handleUserNotADogAction(User user)
        {
            lock (MakedMovePlayerIds)
            {
                if (_roomState.getStateCode() != nameof(PlayingState)) throw new Exception("");
                MakedMovePlayerIds.Add(user.Id);
                _roomState.handleUserNotADogAction(user);
            }

            OnChanged(new RoomChangedEventArgs(this));
        }
    }
}