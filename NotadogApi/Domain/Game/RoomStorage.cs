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
            PlayersMaxCount = room.PlayersMaxCount.Value;
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
            _userRoomMap.AddOrUpdate(user.Id, room, (k, v) => room);

            return room;
        }

        public async Task<Room> AddUserToAvailableRoom(User user, int playersMaxCount)
        {
            var key = new PublicRoomKey { PlayersMaxCount = playersMaxCount };
            var availableRoom = _publicRooms.ContainsKey(key) ? _publicRooms[key] : null;
            if (availableRoom != null) return await AddUserToRoom(user, availableRoom, false);

            var newRoom = new Room(playersMaxCount);

            var roomKey = new PublicRoomKey(newRoom);
            _publicRooms.TryAdd(roomKey, newRoom);

            newRoom.Changed += HandleRoomChanged;
            return await AddUserToRoom(user, newRoom, false);
        }

        public async Task<Room> CreatePrivateRoom(User user)
        {
            var newRoom = new Room();
            var room = await AddUserToRoom(user, newRoom, false);
            _privateRooms.TryAdd(room.Guid, newRoom);
            room.RootId = user.Id;
            room.Changed += HandleRoomChanged;

            return room;
        }

        public Task RemoveUserFromRoom(User user, Room room)
        {
            _userRoomMap.TryRemove(user.Id, out _);
            room?.removePlayer(user);

            return Task.CompletedTask;
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
            TryRemoveRoom(room);

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

            switch (e.room.getStateCode())
            {
                case nameof(WaitingStartState):
                    TryRemoveRoom(e.room);
                    break;
                case nameof(EndState):
                    RemoveRoom(e.room);
                    break;
                default:
                    break;
            }
        }

        private void TryRemoveRoom(Room room)
        {
            if (room.isPublic())
            {
                _publicRooms.TryRemove(new PublicRoomKey(room), out _);
            }
            else
            {
                _privateRooms.TryRemove(room.Guid, out _);
            }
        }
    }
}

