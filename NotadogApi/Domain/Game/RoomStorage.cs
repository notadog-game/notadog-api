using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    struct RoomKey
    {
        public string Guid;

        public RoomKey(string guid)
        {
            Guid = guid;
        }
    }

    public class RoomStorage : IRoomStorage
    {
        private ConcurrentDictionary<RoomKey, Room> _publicRooms;
        private ConcurrentDictionary<string, Room> _userRoomMap;
        public event EventHandler<RoomChangedEventArgs> Changed;

        protected virtual void OnChanged(RoomChangedEventArgs e)
        {
            EventHandler<RoomChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, e);
        }

        public RoomStorage()
        {
            _publicRooms = new ConcurrentDictionary<RoomKey, Room>();
            _userRoomMap = new ConcurrentDictionary<string, Room>();
        }

        public async Task<Room> AddUserToRoom(User user, Room room, Boolean forceAdding)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            if (existingUserRoom != null)
            {
                if (!forceAdding)
                {
                    return null;
                }
                else
                {
                    existingUserRoom.removePlayer(user);
                }
            }

            room.addPlayer(user);
            _userRoomMap.AddOrUpdate(user.Id.ToString(), room, (k, v) => v);

            return room;
        }

        public async Task<Room> AddUserToAvailableRoom(User user, Boolean forceAdding)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            if (existingUserRoom != null)
            {
                if (!forceAdding)
                {
                    return null;
                }
                else
                {
                    existingUserRoom.removePlayer(user);
                }
            }

            var availableRoom = _publicRooms.FirstOrDefault().Value;
            if (availableRoom != null) return await AddUserToRoom(user, availableRoom, forceAdding);

            var newRoom = CreateRoom(_publicRooms);
            return await AddUserToRoom(user, newRoom, forceAdding);
        }

        public async Task<Room> RemoveUserFromRoom(User user)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            existingUserRoom.removePlayer(user);

            Room room;
            _userRoomMap.TryRemove(user.Id.ToString(), out room);

            return room;
        }

        public Task<Room> GetRoomByUserId(int userId)
        {
            var key = _userRoomMap.Keys.FirstOrDefault(k => k == userId.ToString());
            if (key == null) return Task.FromResult<Room>(null);

            Room room;
            if (_userRoomMap.TryGetValue(key, out room)) return Task.FromResult(room);

            return Task.FromResult<Room>(null);
        }

        private Room CreateRoom(ConcurrentDictionary<RoomKey, Room> storage)
        {
            var room = new Room();
            var roomKey = new RoomKey(room.getGuid());

            if (!storage.TryAdd(roomKey, room))
            {
                /* TODO: Throw error */
                return null;
            }

            room.Changed += HandleRoomChanged;
            return room;
        }

        private Room RemoveRoom(Room room, ConcurrentDictionary<RoomKey, Room> storage)
        {
            var roomKey = new RoomKey(room.getGuid());

            Room outRoom;
            if (!storage.TryRemove(roomKey, out outRoom))
            {
                /* TODO: Throw error */
                return null;
            }

            foreach (var user in outRoom.getPlayers())
            {
                _userRoomMap.TryRemove(user.Id.ToString(), out _);
            };

            room.Changed += HandleRoomChanged;
            return outRoom;
        }

        private void HandleRoomChanged(object sender, RoomChangedEventArgs e)
        {
            OnChanged(new RoomChangedEventArgs(e.room));
        }
    }
}

