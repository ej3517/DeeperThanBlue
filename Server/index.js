const express = require('express'); 
const bodyParser = require("body-parser"); 
const app = express(); 

app.use(bodyParser.json()); 
const path = require('path'); 

const db = require("./db.js"); 
const collection = "todo"; 

var io = require('socket.io')(process.env.PORT || 4567);




console.log('Server has started');

db.connect((err)=>{
      if(err){
          console.log('unable to connect to database'); 
          process.exit(1);
      }
      else {
          app.listen(8000, ()=>{
              console.log('connected to database, app listening on port 8000'); 
          })
      }
});

// On connection update the database from the game 
io.on('connection', function(socket)
{
      console.log('Connection made')

      // Send leaderboard Json 

      app.get('/getTodos', (req,res)=>{
            const leaderboard = req.body; 
            console.log(leaderboard); 
      });

      // Send questions from db 

      socket.on('disconnect', function() 
      {
            console.log('A player has disconnected')
            socket.broadcast.emit('disconnected', player); 
      }); 

})
