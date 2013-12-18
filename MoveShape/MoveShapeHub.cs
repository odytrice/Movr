using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Movr.MoveShape
{
    /// <summary>
    /// A Hub is a thing that clients connect to to talk to each other
    /// The server can also use the hub to broadcast out to connected clients
    /// </summary>
    [HubName("MoveShape")]
    public class MoveShapeHub : Hub
    {
        private static readonly ConcurrentDictionary<string, object> _connections = new ConcurrentDictionary<string, object>();
        //Server Side Method to Move the Shape of the Global State
        public void MoveShape(int x, int y)
        {
            ///Call Shaped Moved on Client Code
            ///Passing the current connectionID and the co-ordinates
            Clients.All.ShapeMoved(Context.ConnectionId, x, y);
        }

        public override Task OnConnected()
        {
            _connections.TryAdd(Context.ConnectionId, null);
            Clients.All.ClientCountChanged(_connections.Count);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            _connections.TryAdd(Context.ConnectionId, null);
            Clients.All.ClientCountChanged(_connections.Count);
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            object value;
            _connections.TryRemove(Context.ConnectionId, out value);
            Clients.All.ClientCountChanged(_connections.Count);
            return base.OnDisconnected();
        }
    }
}