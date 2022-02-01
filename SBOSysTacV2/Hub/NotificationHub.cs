using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Hub
{
    public class NotificationHub: Microsoft.AspNet.SignalR.Hub
    {
        private static readonly ConcurrentDictionary<string,UserHubViewModel> Users=new ConcurrentDictionary<string, UserHubViewModel>(StringComparer.InvariantCultureIgnoreCase);

        public override Task OnConnected()
        {
            string _userName = Context.User.Identity.Name;
            string _connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(_userName, _ => new UserHubViewModel()
            {
                username = _userName,
                connectionId = new HashSet<string>()
            });

            lock (user.connectionId)
            {
                user.connectionId.Add(_connectionId);

                if (user.connectionId.Count == 1)
                {
                    Clients.Others.userConnected(_userName);
                }

            }
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            string _userName = Context.User.Identity.Name;
            string _connectionId = Context.ConnectionId;

            UserHubViewModel user;
            Users.TryGetValue(_userName, out user);

            if (user != null)
            {
                lock (user.connectionId)
                {
                    user.connectionId.RemoveWhere(cid => cid.Equals(_connectionId));
                    if (!user.connectionId.Any())
                    {
                        UserHubViewModel removedUser;
                        Users.TryRemove(_userName, out removedUser);
                        Clients.Others.userDisconnected(_userName);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }


    }
}