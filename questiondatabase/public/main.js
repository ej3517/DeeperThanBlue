//NOT NEEDED NOW

const update = document.querySelector('#update-button')
const deleteButton = document.querySelector('#delete-button')
const messageDiv = document.querySelector('#message')

//used for update
update.addEventListener('click', _ => {
  fetch('/translation', {
    method: 'put',
    //telling server we're sending JSON data
    headers: { 'Content-Type': 'application/json' },
    //converting data to send to JSON
    body: JSON.stringify({
      input: 'Hi',
      translation: 'Hola'
    })
  })
    .then(res => {
      if (res.ok) return res.json()
    })
    .then(response => {
      window.location.reload(true)
    })
})

//used for delete
deleteButton.addEventListener('click', _ => {
  fetch('/translation', {
    method: 'delete',
    headers: { 'Content-Type': 'application/json' },
    //just deleting Hi translation
    body: JSON.stringify({
      input: 'Hi'
    })
  })
    .then(res => {
      if (res.ok) return res.json()
    })
    .then(response => {
      if (response === 'No translation to delete') {
        messageDiv.textContent = 'No Hi translation to delete'
      } else {
        window.location.reload(true)
      }
    })
    .catch(console.error)
})
