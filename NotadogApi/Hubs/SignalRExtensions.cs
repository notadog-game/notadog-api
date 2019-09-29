using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NotadogApi.Hubs
{
    public static class SignalRExtensions
    {
        public static T Users<T, TUserId>(this IHubClients<T> clients, IEnumerable<TUserId> userIds)
        {
            var strUserIds = userIds.Select(id => id.ToString())
                .ToList();
            return clients.Users(strUserIds);
        }

        public static T User<T, TUserId>(this IHubClients<T> clients, TUserId userId) =>
            clients.User(userId.ToString());

        public static Task SendAsync<T>(this IClientProxy clientProxy, T method, object arg) =>
            // We can't just clientProxy.SendAsync(..), because it's also extension method.
            // It will use this declaring method as clientProxy.SendAsync(..) and will make endless recursion.
            ClientProxyExtensions.SendAsync(clientProxy, method.ToString(), arg);
    }
}
