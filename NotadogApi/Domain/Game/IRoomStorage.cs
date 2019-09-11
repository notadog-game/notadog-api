using System;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public interface IRoomStorage
    {
        event EventHandler<RoomChangedEventArgs> Changed;
        Task<Room> AddUserToRoom(User user, Room room, Boolean forceAdding);
        Task<Room> AddUserToAvailableRoom(User user, Boolean forceAdding);
        Task<Room> CreatePrivateRoom(User user, Boolean forceAdding);
        Task<Room> GetPrivateRoomById(string userId);
        Task<Room> GetRoomByUserId(int userId);
        Task RemoveUserFromRoom(User user);
    }
}

//TODO: think about multi getting free room