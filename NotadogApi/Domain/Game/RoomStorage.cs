using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Game.States;

namespace NotadogApi.Domain.Game
{
    struct PublicRoomKey
    {
        public int PlayersMaxCount;

        public PublicRoomKey(Room room)
        {
            PlayersMaxCount = room.PlayersMaxCount;
        }
    }

    public class RoomStorage : IRoomStorage
    {
        private ConcurrentDictionary<Guid, Room> _privateRooms;
        private ConcurrentDictionary<PublicRoomKey, Room> _publicRooms;
        private ConcurrentDictionary<int, Room> _userRoomMap;
        public event EventHandler<RoomChangedEventArgs> Changed;

        public RoomStorage()
        {
            _privateRooms = new ConcurrentDictionary<Guid, Room>();
            _publicRooms = new ConcurrentDictionary<PublicRoomKey, Room>();
            _userRoomMap = new ConcurrentDictionary<int, Room>();
        }

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        public async Task<Room> AddUserToRoom(User user, Room room, Boolean forceAdding)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            // TODO: Create typed exception
            if (existingUserRoom != null && !forceAdding) throw new Exception("");
            existingUserRoom?.removePlayer(user);

            room.addPlayer(user);
            _userRoomMap.AddOrUpdate(user.Id, room, (k, v) => v);

            return room;
        }

        public async Task<Room> AddUserToAvailableRoom(User user, Boolean forceAdding)
        {
            var availableRoom = _publicRooms.FirstOrDefault().Value;
            if (availableRoom != null) return await AddUserToRoom(user, availableRoom, forceAdding);

            var newRoom = new Room();

            var roomKey = new PublicRoomKey(newRoom);
            _publicRooms.TryAdd(roomKey, newRoom);

            newRoom.Changed += HandleRoomChanged;
            return await AddUserToRoom(user, newRoom, forceAdding);
        }

        public async Task<Room> CreatePrivateRoom(User user, Boolean forceAdding)
        {
            var newRoom = new Room();
            var room = await AddUserToRoom(user, newRoom, forceAdding);
            _privateRooms.TryAdd(room.Guid, room);
            room.Changed += HandleRoomChanged;

            return room;
        }

        public async Task RemoveUserFromRoom(User user)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            existingUserRoom?.removePlayer(user);

            _userRoomMap.TryRemove(user.Id, out _);
        }

        public Task<Room> GetRoomByUserId(int userId)
        {
            return Task.FromResult(_userRoomMap.ContainsKey(userId) ? _userRoomMap[userId] : null);
        }

        public Task<Room> GetPrivateRoomById(string roomId)
        {
            var _roomId = Guid.Parse(roomId);
            return Task.FromResult(_privateRooms.ContainsKey(_roomId) ? _privateRooms[_roomId] : null);
        }

        private Room RemoveRoom(Room room)
        {
            _publicRooms.TryRemove(new PublicRoomKey(room), out _);
            _privateRooms.TryRemove(room.Guid, out _);

            foreach (var user in room.Players)
            {
                _userRoomMap.TryRemove(user.Id, out _);
            };

            room.Changed -= HandleRoomChanged;
            return room;
        }

        private void HandleRoomChanged(object sender, RoomChangedEventArgs e)
        {
            OnChanged(new RoomChangedEventArgs(e.room));
            if (e.room.getStateCode() == nameof(EndState)) RemoveRoom(e.room); ;
        }
    }
}

