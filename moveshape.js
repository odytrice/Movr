/// <reference path="Scripts/jquery-1.6.4.js" />
/// <reference path="Scripts/jquery.signalR-2.0.1.js" />
/// <reference path="Scripts/jquery-ui-1.10.3.js" />

$(function () {
    //Create Representation of the Server's Move Shape
    var hub = $.connection.MoveShape,
        $shape = $("#shape"),
        $clients = $("#clientCount");

    //Extend the Hub and add shapeMoved that is to be called from the server
    $.extend(hub.client, {
        ShapeMoved: function (cid, x, y) {
            //Update the position of the object on the screen
            $shape.css({ left: x, top: y });
        },
        ClientCountChanged: function (count) {
            console.log("Clients is now " + count);
            $clients.html(count);
        },
        Initialize: function (x, y) {
            $shape.css({ left: x, top: y, visibility: "visible" });
        }
    });


    ///I have to start the connection with the server and then wire up the client behaviour
    ///hub.start() is asynchronous
    $.connection.hub.start().done(function () {
        //Make the shape draggable
        $shape.draggable({
            //When the drag takes place, call the method moveshape on the Hub
            drag: function () {
                hub.server.moveShape(this.offsetLeft, this.offsetTop || 0)
            }
        })
    });
});