var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

console.log("server started on port " + port);

var players = [];

io.on("connection", function(socket) {
    
    var currentPlayerId = shortId.generate();
    var player = 
    {
        id:currentPlayerId,
        position:{x:0, y:0}
    };
    players[currentPlayerId] = player

    socket.on("user connect", function(){
        console.log("user connected");
        for(var i = 0; i < clients.length; i++){
            socket.emit("user connected", {name:clients[i].name, position:clients[i].position});
            console.log("User name" + clients[i].name + " is connected");
        }
    });

    socket.on("play", function(data) {
        currentUser = {
            name:data.name,
            position:data.position
        }
        clients.push(currentUser);
        socket.emit("play", currentUser);
        socket.broadcast.emit("user connected", currentUser);
    });

    socket.on("move", function(data) {
        currentUser.position = data.position;
        socket.emit("Move", currentUser);
        socket.broadcast.emit("Move", currentUser);
        console.log(currentUser.name + " move to " + currentUser.position);
    });

    socket.on("disconnect", function(data) {
        socket.broadcast.emit("User Disconnected", currentUser);
        for(var i = 0; i < clients.length; i++) {
            if(clients[i].name == currentUser.name) {
                console.log("User " + clients[i].name + " is logged out");
                clients.splice(i, 1);
            }
        };
    });
});