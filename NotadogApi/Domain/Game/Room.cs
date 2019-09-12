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
        public List<User> MakedMovePlayers { get; }
        public int PlayersMaxCount { get; }
        public int RootId { get; set; }
        private IRoomState _roomState;

        public event EventHandler<RoomChangedEventArgs> Changed;

        public Room(int playersMaxCount)
        {
            Guid = Guid.NewGuid();
            Players = new List<User>();
            MakedMovePlayers = new List<User>();
            PlayersMaxCount = playersMaxCount;

            _roomState = new WaitingPlayersState(this);
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        public string getStateCode()
        {
            return _roomState.getStateCode();
        }

        public int getPlayersCount()
        {
            return Players.Count;
        }

        public void changeState(IRoomState state)
        {
            _roomState = state;
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void addPlayer(User user)
        {
            // TODO: Implement exception
            if (Players.Any(player => player.Id == user.Id)) throw new Exception("");

            Players.Add(user);

            if (PlayersMaxCount > 0 && PlayersMaxCount == Players.Count)
            {
                changeState(new WaitingStartState(this));
                return;
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void removePlayer(User user)
        {
            var player = Players.Single(p => p.Id == user.Id);
            Players.Remove(player);
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void start(User user)
        {
            if (RootId != user.Id) return;
            if (_roomState.getStateCode() != nameof(WaitingPlayersState)) return;

            changeState(new WaitingStartState(this));
        }

        public void handleUserNotADogAction(User user)
        {
            if (_roomState.getStateCode() != nameof(PlayingState)) return;

            MakedMovePlayers.Add(user);
            _roomState.handleUserNotADogAction(user);

            OnChanged(new RoomChangedEventArgs(this));
        }
    }
}