using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NotadogApi.Hubs
{
    public class GameHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("OnConnect", $"{Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("OnDisconnect", $"{Context.ConnectionId}");
        }
    }
}