using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotadogApi.Domain.Game;
using NotadogApi.Infrastructure;
using NotadogApi.Domain.Exceptions;

namespace NotadogApi.Hubs
{
    [Authorize]
    public class GameHub : GameHubBase
    {
        public GameHub(ICurrentUserAccessor currentUserAccessor, IRoomStorage roomStorage) : base(currentUserAccessor, roomStorage) { }

        public override async Task StartGame() => await Invoke(base.StartGame);

        public override async Task MakeMove() => await Invoke(base.MakeMove);

        public override async Task LeaveRoom() => await Invoke(base.LeaveRoom);

        public override async Task Refresh() => await Invoke(base.Refresh);

        public override async Task Replay() => await Invoke(base.Replay);

        public override async Task OnConnectedAsync() => await Invoke(base.OnConnectedAsync);

        public override async Task OnDisconnectedAsync(Exception exception) => await Invoke(() => base.OnDisconnectedAsync(exception));

        private async Task Invoke(Func<Task> func)
        {
            try { await func(); }
            catch (CommonException exception) { throw new HubException(exception.Error.ToJson()); }
        }
    }
}