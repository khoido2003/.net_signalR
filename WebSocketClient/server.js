const http = require('http');
const fs = require('fs');

const PORT=8082; 

fs.readFile('./WebSocket.html', function (err, html) {

    if (err) throw err;    

    http.createServer(function(request, response) {  
        response.writeHeader(200, {"Content-Type": "text/html"});  
        response.write(html);
        response.end();  
      }).listen(PORT);
    });
console.log("Webpage running on http://localhost:8082/");  