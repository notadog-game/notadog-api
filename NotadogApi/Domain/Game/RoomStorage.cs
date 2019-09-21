using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game.States;

namespace NotadogApi.Domain.Game
{
    public class RoomStorage : IRoomStorage
    {
        private ConcurrentDictionary<int, Room> _hashRoomMap;
        private ConcurrentDictionary<int, Room> _userRoomMap;
        public event EventHandler<RoomChangedEventArgs> Changed;
        private Timer _timer;

        public RoomStorage()
        {
            _hashRoomMap = new ConcurrentDictionary<int, Room>();
            _userRoomMap = new ConcurrentDictionary<int, Room>();

            _timer = new Timer {Interval = 60000, AutoReset = true, Enabled = true};
            _timer.Elapsed += OnTimedEvent;
        }

        public async Task<Room> CreateRoom(User user)
        {
            var newRoom = new Room();
            var room = await JoinRoom(user, newRoom, false);
            _hashRoomMap.TryAdd(getHashCode(room.Guid.ToString()), newRoom);
            room.RootId = user.Id;
            room.Changed += HandleRoomChanged;

            return room;
        }

        public async Task<Room> JoinRoom(User user, Room room, bool forceAdding)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            // TODO: Create typed exception
            if (existingUserRoom != null && !forceAdding) throw new Exception("");
            existingUserRoom?.RemovePlayer(user);

            room.AddPlayer(user);
            _userRoomMap.AddOrUpdate(user.Id, room, (k, v) => room);

            return room;
        }

        public async Task<Room> JoinAvailableRoom(User user, int playersMaxCount)
        {
            var key = getHashCode(playersMaxCount, 0);
            var availableRoom = _hashRoomMap.ContainsKey(key) ? _hashRoomMap[key] : null;
            if (availableRoom != null) return await JoinRoom(user, availableRoom, false);

            var newRoom = new Room(playersMaxCount);
            _hashRoomMap.TryAdd(key, newRoom);

            newRoom.Changed += HandleRoomChanged;
            return await JoinRoom(user, newRoom, false);
        }

        public async Task<Room> LeaveRoom(User user)
        {
            var room = await GetRoomByUserId(user.Id);
            _userRoomMap.TryRemove(user.Id, out _);
            room?.RemovePlayer(user);

            return room;
        }

        private void DestroyRoom(Room room)
        {
            RemoveRoom(room);
            foreach (var user in room.Players)
                _userRoomMap.TryRemove(user.Id, out _);

            room.Changed -= HandleRoomChanged;
        }

        private void RemoveRoom(Room room)
        {
            var key = room.IsPublic() ? getHashCode(room.PlayersMaxCount.Value, 0) : getHashCode(room.Guid.ToString());
            _hashRoomMap.TryRemove(key, out _);
        }

        public Task<Room> GetRoomByUserId(int userId) => Task.FromResult(_userRoomMap.ContainsKey(userId) ? _userRoomMap[userId] : null);

        private Task<Room> GetRoomByKey(int key) => Task.FromResult(_hashRoomMap.ContainsKey(key) ? _hashRoomMap[key] : null);

        public Task<Room> GetRoomByPayload(string roomGuid) => GetRoomByKey(getHashCode(roomGuid));

        public Task<Room> GetRoomByPayload(int playersMaxCount, int bet) => GetRoomByKey(getHashCode(playersMaxCount, bet = 0));

        private int getHashCode(string roomGuid) => roomGuid.GetHashCode();

        private int getHashCode(int playersMaxCount, int bet) => $"{playersMaxCount}{bet}".GetHashCode();

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            _hashRoomMap.Keys.ToList().ForEach(key =>
            {
                var room = _hashRoomMap[key];

                var timestamp = room.GetStateTimestamp();
                var diff = e.SignalTime.Subtract(timestamp).TotalMilliseconds;

                switch (room.GetStateCode())
                {
                    case nameof(PlayingState):
                        if (diff > 60000) room.ChangeState(new EndState(room));
                        break;
                    case (nameof(EndState)):
                        if (diff > 60000) DestroyRoom(room);
                        break;
                    default:
                        break;
                }
            });
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void HandleRoomChanged(object sender, RoomChangedEventArgs e)
        {
            OnChanged(new RoomChangedEventArgs(e.room));
        }
    }
}

