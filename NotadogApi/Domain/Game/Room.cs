using System;
using System.Collections.Generic;

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
        private readonly System.Guid _guid;
        private readonly List<User> _users;
        private IRoomState _roomState;
        private readonly int _maxPlayersCount;
        public event EventHandler<RoomChangedEventArgs> Changed;

        public Room()
        {
            _guid = Guid.NewGuid();
            _users = new List<User>();
            _maxPlayersCount = 2;
            _roomState = new WaitingPlayersState(this);
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        public string getGuid()
        {
            return this._guid.ToString();
        }

        public string getStateCode()
        {
            return _roomState.getStateCode();
        }

        public int getPlayersMaxCount()
        {
            return _maxPlayersCount;
        }

        public int getPlayersCount()
        {
            return _users.Count;
        }

        public IEnumerable<User> getPlayers()
        {
            return _users;
        }

        public void changeState(IRoomState state)
        {
            _roomState = state;
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void addPlayer(User user)
        {
            _users.Add(user);
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void removePlayer(User user)
        {
            _users.Remove(user);
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void handleUserNotADogAction(User user)
        {
            _roomState.handleUserNotADogAction(user);
            OnChanged(new RoomChangedEventArgs(this));
        }
    }
}