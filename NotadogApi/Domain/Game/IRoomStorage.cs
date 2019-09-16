using System;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public interface IRoomStorage
    {
        event EventHandler<RoomChangedEventArgs> Changed;
        Task<Room> AddUserToRoom(User user, Room room, Boolean forceAdding);
        Task<Room> AddUserToAvailableRoom(User user);
        Task<Room> CreatePrivateRoom(User user);
        Task<Room> GetPrivateRoomById(string userId);
        Task<Room> GetRoomByUserId(int userId);
        Task RemoveUserFromRoom(User user, Room room);
    }
}

//TODO: think about multi getting free room