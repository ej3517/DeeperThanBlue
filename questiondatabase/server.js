const express = require('express');
const bodyParser= require('body-parser')
const app = express();
const MongoClient = require('mongodb').MongoClient

const connectionString = 'mongodb+srv://nl1117:deeperthanblue@cluster0-yx39o.mongodb.net/test?retryWrites=true&w=majority'

MongoClient.connect(connectionString, { useUnifiedTopology: true })
  .then(client => {

    //console.log('Connected to Database')
    const db = client.db('translation')
    //storing translation
    const translationCollection = db.collection('translation')

    // ========================
    // Middlewares
    // ========================

    //tells express we are using embedded js as template engine for html
    app.set('view engine', 'ejs')
    //for put request
    app.use(express.static('public'))
    //allows server to accept JSON data
    app.use(bodyParser.json())


    //reading from localhost:3000
    app.get('/', (req, res) => {
      db.collection('translation').find().toArray()
        .then (results => {
          //render: generate html that contains translation
          //res.render(file we're rendering, data passed into the file)
          res.render('index.ejs',{translation: results})
      })
      .catch(error => console.error(error))
    })

    //create
    app.post('/translation', (req, res) => {
      //insertOne adds items to a mongoDB collection
      translationCollection.insertOne(req.body)
        .then(result => {
          res.redirect('/')
        })
        .catch(error => console.error(error))
    })

    //NOT NEEDED NOW: update
    app.put('/translation', (req,res) => {
      console.log(req.body)
      translationCollection.findOneAndUpdate(
        //query: lets us filter the collection
        { input: 'a' },
        //update: tells MongoDB what to change
        {
          //other update operators like push and inc
          $set: {
            input: req.body.input,
            translation: req.body.translation
          }
        },
        //option: additional options for update request
        {
          //upsert: inserts a document if no docs can be updated
          upsert: true
        }
      )
      .then(result => {
        res.json('Success')
       })
      .catch(error => console.error(error))
    })

    //NOT NEEDED NOW: delete
    app.delete('/translation', (req, res) => {
      translationCollection.deleteOne(
        //query: removing documents with darth vader as the input
        //since we already pass the input Hi from fetch
        //we dont need to hardcode it again
        { input: req.body.input }
        //would add options here, but none needed in this case
      )
        .then(result => {
          if (result.deletedCount === 0) {
            return res.json('No translation to delete')
          }
          res.json('Deleted Hi translation')
        })
        .catch(error => console.error(error))
    })

  })
  .catch(error => console.error(error))


//body-bodyParser
app.use(bodyParser.urlencoded({ extended: true }))

app.listen(3000, function() {
  console.log('listening on 3000')
})

//app.get('/', (req, res) => {
//  res.sendFile(__dirname + '/index.html')
//})
