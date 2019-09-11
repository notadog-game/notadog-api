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
        public Guid Guid { get; }
        public List<User> Players { get; }
        public int PlayersMaxCount { get; }
        private IRoomState _roomState;

        public event EventHandler<RoomChangedEventArgs> Changed;

        public Room()
        {
            Guid = Guid.NewGuid();
            Players = new List<User>();
            PlayersMaxCount = 2;

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
            Players.Add(user);
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void removePlayer(User user)
        {
            Players.Remove(user);
            OnChanged(new RoomChangedEventArgs(this));
        }

        public void handleUserNotADogAction(User user)
        {
            _roomState.handleUserNotADogAction(user);
            OnChanged(new RoomChangedEventArgs(this));
        }
    }
}