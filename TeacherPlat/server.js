var http = require('http'); 
var fs = require('fs');

var express = require('express'); 
var app = express();

app.listen(3000, () => console.log('listening to port 3000')); 
app.use(express.static('public'));

console.log('Hello');

app.post('/api', (request, response) => {
    console.log(request); 
}); 

function onRequest(request, response) {
    response.writeHead(200, {'Content-type': 'text/html'});
    fs.readFile('./indexServer.html', null, function(error, data) 
    {
        if (error){
            response.writeHead(404);
            response.write('File not found!');
        } else {

            response.write(data); 
        }
        response.end();
    })

}


http.createServer(onRequest).listen(3000);