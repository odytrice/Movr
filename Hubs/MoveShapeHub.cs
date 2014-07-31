using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Drawing;

namespace Movr.Hubs
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
            //Move the Shape on the Server
            SetShape(x, y);

            ///Notify Everyone that the Shape has moved
            ///Passing the current connectionID and the co-ordinates
            Clients.Others.ShapeMoved(Context.ConnectionId, x, y);
        }

        private void SetShape(int x, int y)
        {
            //Set Shape's Location
            _location = new Point(x, y);
        }

        //This happens whenever a New Client Connects
        public override Task OnConnected()
        {
            NewConnection();

            //Return SignlaR's On Connected Method as a Task
            return base.OnConnected();
        }

        //This happens whenever a New Client Regains Connection
        public override Task OnReconnected()
        {
            NewConnection();

            //Return SignlaR's On Reconnected Method as a Task
            return base.OnReconnected();
        }

        
        private void NewConnection()
        {
            //Add to Connections Dictionary
            _connections.TryAdd(Context.ConnectionId, null);

            //Notify All Clients that a new Client has been connected
            Clients.All.ClientCountChanged(_connections.Count);

            //Bring Current up to Speed with latest coordinates
            Clients.Client(Context.ConnectionId).Initialize(_location.X, _location.Y);
        }

        //This Happens Whenever a Client is Disconnected
        public override Task OnDisconnected()
        {
            object value;
            //Try to Remove the ConnectionID from the Dictionary
            _connections.TryRemove(Context.ConnectionId, out value);

            //Notify Everyone that the client count has reduced
            Clients.All.ClientCountChanged(_connections.Count, _location.X, _location.Y);
            return base.OnDisconnected();
        }
    }
}