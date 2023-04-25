"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    var today = new Date();
    var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
    var hours = today.getHours();
    var minutes = today.getMinutes();
    var AmOrPm = (hours >= 12) ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var time = hours + ":" + minutes + ":" + today.getSeconds();
    var dateTime = date + ' ' + time + ' ' + AmOrPm;
    li.textContent = `${user} - ${dateTime} : ${message}`;
    document.getElementById("messageInput").value = "";
});

connection.start().then(function () {
    var contactId = document.getElementById("contactIdInput").innerHTML;
    connection.invoke("AddToGroup", contactId );
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").innerHTML;
    var message = document.getElementById("messageInput").value;
    var contactId = document.getElementById("contactIdInput").innerHTML;
    var token = document.getElementById("Token").innerHTML;
    connection.invoke("SendMessage", user, message, contactId, token).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});