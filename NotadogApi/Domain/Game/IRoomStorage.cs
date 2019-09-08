using System;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public interface IRoomStorage
    {
        Task<Room> AddUserToRoom(User user, Room room, Boolean forceAdding);
        Task<Room> AddUserToAvailableRoom(User user, Boolean forceAdding);
        Task<Room> GetRoomByUserId(int userId);
        Task<Room> RemoveUserFromRoom(User user);
    }
}

//TODO: think about multi getting free room