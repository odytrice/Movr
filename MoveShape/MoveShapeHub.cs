using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movr.MoveShape
{
    /// <summary>
    /// A Hub is a thing that clients connect to to talk to each other
    /// The server can also use the hub to broadcast out to connected clients
    /// </summary>
    [HubName("MoveShape")]
    public class MoveShapeHub : Hub
    {
        //Server Side Method to Move the Shape of the Global State
        public void MoveShape(int x, int y)
        {
            ///Call Shaped Moved on Client Code
            ///Passing the current connectionID and the co-ordinates
            Clients.All.shapeMoved(Context.ConnectionId, x, y);
        }
    }
}