
//-----------------//
//                 //
//     STEP 00     //
//                 //
//-----------------//
// Define variables
var clientName = '';
var contactName = '';

//-----------------//
//                 //
//     STEP 01     //
//                 //
//-----------------//
// Initialize SignalR
var connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/chat-hub")
    .build();

//-----------------//
//                 //
//     STEP 02     //
//                 //
//-----------------//
// Start connection
var startConnection = function () {
    connection
        .start()
        //-----------------//
        //                 //
        //     STEP 03     //
        //                 //
        //-----------------//
        .then(setRoom)
        .catch(function (err) {
            console.log(err);
        });
}


//-----------------//
//                 //
//     STEP 03     //
//                 //
//-----------------//
// Set room
var setRoom = function() {
    connection.invoke('SetChatRoomAsync', clientName, contactName);
}

//-----------------//
//                 //
//     STEP 04     //
//                 //
//-----------------//
// Send message
var sendMessageBtn = document.getElementById('send-message-button');
if (sendMessageBtn) {
    sendMessageBtn.addEventListener('click', function (e) {
        e.preventDefault();
        var messageInputElement = document.getElementById('chat-message');
        if (messageInputElement) {
            var message = messageInputElement.value;
            if (message && message.length) {
                connection.invoke('SendMessageToUserAsync', clientName, message);
                messageInputElement.value = '';
            }
        }
    });
}

//-----------------//
//                 //
//     STEP 05     //
//                 //
//-----------------//
// Receive message
connection.on('receiveMessage', function (senderName, message, dateTime) {
    if (senderName == clientName || senderName == contactName) {
        var messagesListElem = document.querySelector('#client-chat-container ul');
        if (messagesListElem) {
            var senderNameSpanElem = document.createElement('span');
            senderNameSpanElem.className = 'msg-name';
            senderNameSpanElem.textContent = senderName + ' :';

            var messageTextSpanElem = document.createElement('span');
            messageTextSpanElem.className = 'msg-text';
            messageTextSpanElem.textContent = message;

            var dateTimeSpanElem = document.createElement('span');
            dateTimeSpanElem.className = 'msg-date';
            dateTimeSpanElem.textContent = dateTime;

            let newUnorderedListItemElem = document.createElement('li');
            newUnorderedListItemElem.className = senderName == clientName ? 'msg-sent' : 'msg-received';
            newUnorderedListItemElem.appendChild(senderNameSpanElem);
            newUnorderedListItemElem.appendChild(messageTextSpanElem);
            newUnorderedListItemElem.appendChild(dateTimeSpanElem);

            messagesListElem.appendChild(newUnorderedListItemElem);
            messagesListElem.scrollTop = messagesListElem.scrollHeight - messagesListElem.clientHeight;
        }
    }
});

//-----------------//
//                 //
//     STEP 00     //
//                 //
//-----------------//
// Register user
var ready = function () {
    var clientRegisterContainerElem = document.getElementById('client-register-container');
    if (clientRegisterContainerElem) {
        var registerBtnElem = clientRegisterContainerElem.querySelector('#register-button');
        if (registerBtnElem) {
            registerBtnElem.addEventListener('click', function (e) {
                e.preventDefault();

                var clientNameInputElem = clientRegisterContainerElem.querySelector('#client-name');
                if (clientNameInputElem) {
                    clientName = clientNameInputElem.value;
                }

                var contactNameInputElem = clientRegisterContainerElem.querySelector('#contact-name');
                if (contactNameInputElem) {
                    contactName = contactNameInputElem.value;
                }

                var clientChatContainerElem = document.getElementById('client-chat-container');

                if (clientName &&
                    clientName.length &&
                    contactName &&
                    contactName.length &&
                    clientChatContainerElem) {
                    clientRegisterContainerElem.classList.remove('d-block');
                    clientRegisterContainerElem.classList.add('d-none');
                    clientChatContainerElem.classList.remove('d-none');
                    clientChatContainerElem.classList.add('d-block');

                    //-----------------//
                    //                 //
                    //     STEP 02     //
                    //                 //
                    //-----------------//
                    startConnection();
                }
            });
        }
    }
}

//-----------------//
//                 //
//     STEP 00     //
//                 //
//-----------------//
// DOM ready
document.addEventListener('DOMContentLoaded', ready);
