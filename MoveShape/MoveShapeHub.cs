﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Drawing;

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
        private static Point _location = new Point();

        //Server Side Method to Move the Shape of the Global State
        public void MoveShape(int x, int y)
        {
            ///Call Shaped Moved on Client Code
            ///Passing the current connectionID and the co-ordinates
            SetShape(x, y);
            Clients.Others.ShapeMoved(Context.ConnectionId, x, y);
        }

        private void SetShape(int x, int y)
        {
            _location = new Point(x, y);
        }

        public override Task OnConnected()
        {
            _connections.TryAdd(Context.ConnectionId, null);
            Clients.All.ClientCountChanged(_connections.Count);
            Clients.Client(Context.ConnectionId).Initialize(_location.X, _location.Y);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            _connections.TryAdd(Context.ConnectionId, null);
            Clients.All.ClientCountChanged(_connections.Count);
            Clients.Client(Context.ConnectionId).Initialize(_location.X, _location.Y);
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            object value;
            _connections.TryRemove(Context.ConnectionId, out value);
            Clients.All.ClientCountChanged(_connections.Count, _location.X, _location.Y);
            return base.OnDisconnected();
        }
    }
}