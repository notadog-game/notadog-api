using System;
using System.Collections.Generic;
using System.Linq;

using NotadogApi.Domain.Game.States;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Exceptions;

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
            // TODO RootId as parameter.
            PlayersMaxCount = playersMaxCount;
            if (IsPublic() && playersMaxCount < PlayersMinCount) throw new CommonException(ErrorCode.RoomPlayersMaxCountLacked);

            Guid = Guid.NewGuid();
            Players = new List<User>();
            MakedMovePlayerIds = new List<int>();

            _roomState = new WaitingPlayersState(this);
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        public string GetStateCode() => _roomState.StateCode;

        public DateTime GetStateTimestamp() => _roomState.Timestamp;

        public bool IsPublic() => PlayersMaxCount.HasValue;

        public void ChangeState(IRoomState state)
        {
            lock (_roomState)
            {
                _roomState = state;
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void AddPlayer(User user)
        {
            lock (Players)
            {
                if (_roomState.StateCode != nameof(WaitingPlayersState)) throw new CommonException(ErrorCode.RoomNotInWaitingPlayersStateOnAddingPlayer);
                if (Players.Any(player => player.Id == user.Id)) throw new CommonException(ErrorCode.RoomOnAddingExistingPlayer);
                Players.Add(user);

                if (IsPublic() && PlayersMaxCount == Players.Count)
                {
                    ChangeState(new WaitingStartState(this));
                    return;
                }
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void RemovePlayer(User user)
        {
            lock (Players)
            {
                var player = Players.FirstOrDefault(p => p.Id == user.Id);
                Players.Remove(player);
            }

            OnChanged(new RoomChangedEventArgs(this));
        }

        public void Start(User user)
        {
            if (RootId != user.Id) throw new CommonException(ErrorCode.RoomStartingByNonRootPlayer);
            if (_roomState.StateCode != nameof(WaitingPlayersState)) throw new CommonException(ErrorCode.RoomStartingNotInWaitingPlayersState);
            if (Players.Count < PlayersMinCount) throw new CommonException(ErrorCode.RoomStartingNotEnoughPlayers);

            ChangeState(new WaitingStartState(this));
        }

        public void Replay(User user)
        {
            if (RootId != user.Id) throw new CommonException(ErrorCode.RoomReplayingdByNonRootPlayer);
            if (_roomState.StateCode != nameof(EndState)) throw new CommonException(ErrorCode.RoomReplayingNotInEndPlayersState);

            lock (MakedMovePlayerIds)
            {
                MakedMovePlayerIds.Clear();
            }

            ChangeState(new WaitingPlayersState(this));
        }

        public void HandleUserNotADogAction(User user)
        {
            lock (MakedMovePlayerIds)
            {
                if (_roomState.StateCode != nameof(PlayingState)) throw new CommonException(ErrorCode.RoomMakeMoveNotInPlayingState);
                MakedMovePlayerIds.Add(user.Id);
                _roomState.HandleUserNotADogAction(user);
            }

            OnChanged(new RoomChangedEventArgs(this));
        }
    }
}