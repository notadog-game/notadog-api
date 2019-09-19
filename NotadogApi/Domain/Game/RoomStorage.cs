using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game.States;

namespace NotadogApi.Domain.Game
{
    public class RoomStorage : IRoomStorage
    {
        private ConcurrentDictionary<int, Room> _rooms;
        private ConcurrentDictionary<int, Room> _userRoomMap;
        public event EventHandler<RoomChangedEventArgs> Changed;

        public RoomStorage()
        {
            _rooms = new ConcurrentDictionary<int, Room>();
            _userRoomMap = new ConcurrentDictionary<int, Room>();
        }
        public async Task<Room> CreateRoom(User user)
        {
            var newRoom = new Room();
            var room = await JoinRoom(user, newRoom, false);
            _rooms.TryAdd(getHashCode(room.Guid.ToString()), newRoom);
            room.RootId = user.Id;
            room.Changed += HandleRoomChanged;

            return room;
        }
        public async Task<Room> JoinRoom(User user, Room room, Boolean forceAdding)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            // TODO: Create typed exception
            if (existingUserRoom != null && !forceAdding) throw new Exception("");
            existingUserRoom?.removePlayer(user);

            room.addPlayer(user);
            _userRoomMap.AddOrUpdate(user.Id, room, (k, v) => room);

            return room;
        }
        public async Task<Room> JoinRoom(User user, int playersMaxCount)
        {
            var key = getHashCode(playersMaxCount, 0);
            var availableRoom = _rooms.ContainsKey(key) ? _rooms[key] : null;
            if (availableRoom != null) return await JoinRoom(user, availableRoom, false);

            var newRoom = new Room(playersMaxCount);
            _rooms.TryAdd(key, newRoom);

            newRoom.Changed += HandleRoomChanged;
            return await JoinRoom(user, newRoom, false);
        }
        public async Task<Room> LeaveRoom(User user)
        {
            var room = await GetRoomByUserId(user.Id);
            _userRoomMap.TryRemove(user.Id, out _);
            room?.removePlayer(user);

            return room;
        }
        public Task<Room> GetRoomByUserId(int userId) => Task.FromResult(_userRoomMap.ContainsKey(userId) ? _userRoomMap[userId] : null);
        private Task<Room> GetRoomByKey(int key) => Task.FromResult(_rooms.ContainsKey(key) ? _rooms[key] : null);
        public Task<Room> GetRoomByPayload(string roomGuid) => GetRoomByKey(getHashCode(roomGuid));
        public Task<Room> GetRoomByPayload(int playersMaxCount, int bet) => GetRoomByKey(getHashCode(playersMaxCount, bet = 0));
        private int getHashCode(string roomGuid) => roomGuid.GetHashCode();
        private int getHashCode(int playersMaxCount, int bet) => $"{playersMaxCount}{bet}".GetHashCode();
        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }
        private void HandleRoomChanged(object sender, RoomChangedEventArgs e)
        {
            OnChanged(new RoomChangedEventArgs(e.room));
        }
    }
}

