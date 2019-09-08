using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    struct RoomKey
    {
        public int MaxPlayersCount;

        public RoomKey(int maxPlayersCount)
        {
            MaxPlayersCount = maxPlayersCount;
        }
    }

    public class RoomStorage : IRoomStorage
    {
        private ConcurrentDictionary<RoomKey, Room> _publicRooms;
        private ConcurrentDictionary<string, Room> _userRoomMap;

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

            var newRoom = new Room(3);
            var newRoomKey = new RoomKey(newRoom.getPlayersMaxCount());

            if (!_publicRooms.TryAdd(newRoomKey, newRoom))
            {
                /* TODO: Throw error */
                return null;
            }

            return await AddUserToRoom(user, newRoom, forceAdding);
        }

        public async Task<Room> GetRoomByUserId(int userId)
        {
            var key = _userRoomMap.Keys.FirstOrDefault(k => k == userId.ToString());
            if (key == null) return null;

            Room room;
            if (_userRoomMap.TryGetValue(key, out room)) return room;

            return null;
        }

        public async Task<Room> RemoveUserFromRoom(User user)
        {
            var existingUserRoom = await GetRoomByUserId(user.Id);
            if (existingUserRoom != null) existingUserRoom.removePlayer(user);

            return existingUserRoom;
        }
    }
}

