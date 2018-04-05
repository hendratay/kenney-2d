var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

console.log("server started on port " + port);

var players = [];
var playerSpeed =6;

io.on("connection", function(socket) {

    var thisPlayerId;
    var player;

    // Adding player to network
    socket.on('join', function(data){
        thisPlayerId = shortId.generate();
        player = 
        {
            id:thisPlayerId,
            name:data.name,
            health:data.health,
            destination:{x:0, y:0},
            lastPosition:{x:0, y:0},
            lastMoveTime:0
        };
        players[thisPlayerId] = player
        console.log("client connected, id = ", thisPlayerId);
        socket.emit('register', {id:thisPlayerId});

        // Spawn or Inistiate player to network
        socket.broadcast.emit('spawn', {id:thisPlayerId, name:data.name});

        // when connect check if this player has already been inisiate or not
        // If not been inisiate then raise event spawn to unity;
        for(var playerId in players){
            if(playerId == thisPlayerId)
                continue;
            socket.emit('spawn', players[playerId]);
        };
    });

    // player move
    socket.on('move', function (data) {
        data.id = thisPlayerId;
        console.log('client moved', JSON.stringify(data));

        player.destination.x = data.d.x;
        player.destination.y = data.d.y;
        
        var elapsedTime = Date.now() - player.lastMoveTime;
        
        var travelDistanceLimit = elapsedTime * playerSpeed / 1000;
        
        var requestedDistanceTraveled = lineDistance(player.lastPosition, data.c);
        
        player.lastMoveTime = Date.now();
        
        player.lastPosition = data.c;
        
        delete data.c;
        
        data.x = data.d.x;
        data.y = data.d.y;
        
        delete data.d;
        
        socket.broadcast.emit('move', data);
    });

    socket.on('shoot', function(data){
        data.id = thisPlayerId;
        console.log('client shoot', JSON.stringify(data));
		socket.broadcast.emit('shoot', data);
    });

    // when disconect
    socket.on('disconnect', function () {
        console.log('client disconected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id:thisPlayerId});
    });

});

function lineDistance(vectorA, vectorB) {
    var xs = 0;
    var ys = 0;
    
    xs = vectorB.x - vectorA.x;
    xs = xs * xs;
    
    ys = vectorB.y - vectorA.y;
    ys = ys * ys;
    
    return Math.sqrt(xs + ys);
}