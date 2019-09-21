using System;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public interface IRoomState
    {
        string StateCode { get; }
        DateTime Timestamp { get; }
        void HandleUserNotADogAction(User user);
    }
}