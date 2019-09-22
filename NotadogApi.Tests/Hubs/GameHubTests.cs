using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NotadogApi.Domain.Game;
using NotadogApi.Domain.Game.States;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Hubs;
using NotadogApi.Infrastructure;
using NSubstitute;
using NUnit.Framework;

namespace NotadogApi.Tests.Hubs
{
    class GameHubTests
    {
        #region Fields

        private GameHub _gameHub;
        private ICurrentUserAccessor _currentUserAccessor;
        private IRoomStorage _roomStorage;
        private IHubCallerClients _hubCallerClients;
        private IClientProxy _clientProxy;
        private User _user;
        private Room _room;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _currentUserAccessor = Substitute.For<ICurrentUserAccessor>();
            _roomStorage = Substitute.For<IRoomStorage>();
            _hubCallerClients = Substitute.For<IHubCallerClients>();
            _clientProxy = Substitute.For<IClientProxy>();

            _gameHub = new GameHub(_currentUserAccessor, _roomStorage)
            {
                Clients = _hubCallerClients
            };

            _user = new User {Id = "-7"};
            _room = new Room {RootId = _user.Id};
            _room.AddPlayer(_user);
            _room.AddPlayer(new User {Id = "-34"});

            _currentUserAccessor.GetCurrentUserAsync()
                .Returns(Task.FromResult(_user));
            _hubCallerClients.User(_user.Id.ToString())
                .Returns(_clientProxy);
        }

        [Test]
        public async Task StartGame_UserNotInRoom_SendNullRoomPayload()
        {
            await _gameHub.StartGame();

            await _clientProxy.Received()
                // SendAsync cannot be tested for call, because it is static.
                .SendCoreAsync(GameHubMethod.OnRoomUpdate.ToString(), Arg.Is<object[]>(pp => pp.Single() == null));
        }

        [Test]
        public async Task StartGame_UserInRoom_CallsStart()
        {
            _roomStorage.GetRoomByUserId(_user.Id)
                .Returns(Task.FromResult(_room));

            await _gameHub.StartGame();

            Assert.AreEqual(nameof(WaitingStartState), _room.GetStateCode());
        }
    }
}
