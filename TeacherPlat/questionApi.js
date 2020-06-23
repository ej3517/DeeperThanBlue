var XMLHttpRequest = require("xmlhttprequest").XMLHttpRequest;
const fetch = require("node-fetch");

var xhttp = new XMLHttpRequest();
xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
       // Typical action to be performed when the document is ready:
       //document.getElementById("demo").innerHTML = xhttp.responseText;
       //console.dir(document); 
    }
};


var url = "https://www.educationquizzes.com/ks2/maths/addition-and-subtraction-year-3/"; 


xhttp.responseType = 'document';


xhttp.open("GET", url, true);

//xhttp.overrideMimeType('text/xml');

xhttp.onload = () => {
    console.log(xhttp.responseText);
    console.dir(xhttp.responseXML);
}

xhttp.send();