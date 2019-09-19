using System;
using System.Threading.Tasks;

using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game
{
    public interface IRoomStorage
    {
        event EventHandler<RoomChangedEventArgs> Changed;
        Task<Room> CreateRoom(User user);
        Task<Room> JoinRoom(User user, Room room, Boolean forceAdding);
        Task<Room> JoinRoom(User user, int playersMaxCount);
        Task<Room> LeaveRoom(User user);
        Task<Room> GetRoomByUserId(int userId);
        Task<Room> GetRoomByPayload(string roomGuid);
    }
}

//TODO: think about multi getting free room