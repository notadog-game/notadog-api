using System;
using System.Collections.Generic;
using System.Linq;

using NotadogApi.Domain.Game.States;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public class Room
    {
        private readonly System.Guid _guid;
        private readonly List<User> _users;
        private IRoomState _roomState;
        private readonly int _maxPlayersCount;

        public Room(int maxPlayersCount)
        {
            _guid = Guid.NewGuid();
            _maxPlayersCount = maxPlayersCount;
            _users = new List<User>();
            _roomState = new WaitingPlayersState(this);
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
        }

        public void addPlayer(User user)
        {
            _users.Add(user);
        }

        public void removePlayer(User user)
        {
            _users.Remove(user);
        }

        public void handleUserNotADogAction(User user)
        {
            _roomState.handleUserNotADogAction(user);
        }
    }
}