using System;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Game.States
{
    public interface IRoomState
    {
        void handleUserNotADogAction(User user);
        string getStateCode();
    }
}