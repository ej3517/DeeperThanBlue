const express = require('express'); 
const bodyParser = require("body-parser"); 
const app = express(); 

app.use(bodyParser.json()); 
const path = require('path'); 

const db = require("./db.js"); 
const collection = "leaderboard"; 
const question = "questions"; 

app.get('/', (req, res)=>{
    res.sendFile(path.join(__dirname, 'index.html')); 
}); 


// Get leaderboard values from leaderboard collection and publish all
// of them in URL 
// use find() method to filter players  
// players should fill the following params -> score, class, ...
app.get('/getLeaderboard', (req,res)=>{
    db.getDB().collection(collection).find({}).toArray((err, documents)=>{
        if(err) {
            console.log(err);
        }
        else{
            console.log(documents); 
            res.json(documents); 
        }
    })
});


// Nikki and Leo do this for questions 
// question should have 
/* {question
        {
            module: , 
            week: , 
            class: , 
            question: , 
            answer: , 
            lang(if lang question): , 
            translated_lang(same): , 
        }}
*/

//The 404 Route (ALWAYS Keep this as the last route)
app.get('*', function(req, res){
    res.send('what???', 404);
  });


// This method will use the primary id of the database to change its info 
// Can be edited to change current question or for example to fill translation of word 
// after creating entry for language
app.put('/:id', (req,res)=>{
    const userID = req.params.id; 
    const userInput = req.body; 

    db.getDB().collection(collection).findOneAndUpdate({_id : db.getPrimaryKey(userID)},{$set : {score : userInput.score}}, {returnOriginal : false}, (err, result)=>{
        if(err){
            console.log(err); 
        }
        else{
            res.json(result); 
        }
    }); 
})

// This post request adds data to the collection 
// request body carries the json which will be inserted into collection 
app.post('/', (req, res)=>{
    const userInput = req.body; 
    db.getDB().collection(collection).insertOne(userInput, (err, result)=> {
        if(err)
            console.log(err);
        else {
            console.log(result); 
            res.json({result : result, document : result.ops[0]});
        }
            
    });
});

// This will delete one element from the db 
// This is done by a delete request to localhost:5500/:_id
// here i will delete players 
// Nikki and Leo you can add a button to my template or a way to delete questions the teachers do not want 
app.delete('/:id', (req,res)=>{
    const playerID = req.params.id; 

    db.getDB().collection(collection).findOneAndDelete({_id : db.getPrimaryKey(playerID)}, (err, result)=>{
        if(err)
            console.log(err); 
        else {
            console.log(result);
            res.json(result); 
        }
    }); 
})

db.connect((err)=>{
    if(err){
        console.log('unable to connect to database'); 
        process.exit(1);
    }
    else {
        app.listen(5500, ()=>{
            console.log('connected to database, app listening on port 5500'); 
        })
    }
});