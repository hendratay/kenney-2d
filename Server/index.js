var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

console.log("server started on port " + port);

var players = [];

io.on("connection", function(socket) {
    
    var thisPlayerId = shortId.generate();
    var player = 
    {
        id:thisPlayerId,
        position:{x:0, y:0}
    };
    players[thisPlayerId] = player

    console.log("client connected, id = ", thisPlayerId);
   
    // Adding player to network
    socket.emit('register', {id:thisPlayerId});

    // Spawn or Inistiate player to network
    socket.broadcast.emit('spawn', {id:thisPlayerId});
    
    // when connect check if this player has already been inisiate or not
    // If not been inisiate then raise even spawn to unity;
    for(var playerId in players){
        if(playerId == thisPlayerId)
            continue;
        socket.emit('spawn', players[playerId]);
    };
    
    // player move
    socket.on('move', function (data) {
        data.id = thisPlayerId;
        console.log('client moved', JSON.stringify(data));
        
        data.x = data.d.x;
        data.y = data.d.y;
        
        delete data.d;
        
        socket.broadcast.emit('move', data);
    });
    
    // when disconect
    socket.on('disconnect', function () {
        console.log('client disconected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id:thisPlayerId});
    });

});