using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using NotadogApi.Domain.Users.Services;
using NotadogApi.Infrastructure;

namespace NotadogApi.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public GameHub(IUserService userService, ICurrentUserAccessor currentUserAccessor)
        {
            _userService = userService;
            _currentUserAccessor = currentUserAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _userService.GetOneAsync(id);

            await Clients.All.SendAsync("OnConnect", $"{user.Email}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var id = _currentUserAccessor.GetCurrentId();
            var user = await _userService.GetOneAsync(id);

            await Clients.All.SendAsync("OnDisconnect", $"{user.Email}");
        }
    }
}