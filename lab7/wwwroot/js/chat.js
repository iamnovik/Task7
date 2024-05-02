var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var messages = [];
var currentUserId;

connection.on("ReceiveMessage", function (message) {
    console.log(message)
    var li = document.createElement("li");
    li.setAttribute("id", message.senderId=== currentUserId ? "sender" : "receiver");
    li.setAttribute("data-message-id", message.id); // Добавляем атрибут data-message-id для идентификации сообщения
    li.style.position = "relative";
    var span = document.createElement("span");
    span.textContent = message.text;

    li.appendChild(span);

    // Проверяем, является ли отправитель текущим пользователем, и добавляем кнопку "Edit" при необходимости
    if (message.senderId === currentUserId) {
        var editButton = document.createElement("button");
        // Создаем элемент <i> для иконки
        var icon = document.createElement("i");
        icon.className = "fas fa-edit";
        editButton.style.position = "absolute";
        editButton.style.top = "0";
        editButton.style.right = "0";
// Добавляем иконку в кнопку
        editButton.appendChild(icon);
        editButton.className = "edit-button";
        li.appendChild(editButton);
        editButton.addEventListener('click', function() {
            var messageElement = editButton.closest('li'); // Находим родительский элемент сообщения
            var messageTextElement = messageElement.querySelector('span'); // Находим элемент текста сообщения

            var currentText = messageTextElement.textContent; // Получаем текущий текст сообщения
            var newText = prompt('Enter new message:', currentText); // Запрашиваем новый текст у пользователя через диалоговое окно

            // Если пользователь ввел новый текст, обновляем текст сообщения
            if (newText !== null && newText.trim() !== '') {
                messageTextElement.textContent = newText;

                // Отправляем запрос на сервер для обновления сообщения
                var messageId = messageElement.dataset.messageId; // Предполагается, что у вашего элемента сообщения есть атрибут данных messageId
                updateMessageOnServer(messageId, newText);
            }
        });
        
    }

    var messagesList = document.getElementById('messagesList');
    messagesList.appendChild(li);

    window.scrollTo(0, document.body.scrollHeight);
    
});

window.onload = function() {
    window.scrollTo(0, document.body.scrollHeight);
}

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.querySelector('.header i.fa-phone').addEventListener('click', function() {
    var chatId = parseInt(document.getElementById("chatId").value);
    window.location.href = '/VideoCall/Index?chatId=' + chatId;
});

document.getElementById("sendForm").addEventListener("submit", function (event) {
    var senderId = document.getElementById("currentUserId").value;
    currentUserId = senderId;
    var receiverId = document.getElementById("contactId").value;
    var text = document.getElementById("messageInput").value;
    var chatId = parseInt(document.getElementById("chatId").value);
    var message = { chatId : chatId, text: text, senderId: senderId };
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


document.querySelectorAll('.edit-button').forEach(function(button) {
    button.addEventListener('click', function() {
        var messageElement = button.closest('li'); // Находим родительский элемент сообщения
        var messageTextElement = messageElement.querySelector('span'); // Находим элемент текста сообщения

        var currentText = messageTextElement.textContent; // Получаем текущий текст сообщения
        var newText = prompt('Enter new message:', currentText); // Запрашиваем новый текст у пользователя через диалоговое окно
        
        // Если пользователь ввел новый текст, обновляем текст сообщения
        if (newText !== null && newText.trim() !== '') {
            messageTextElement.textContent = newText;

            // Отправляем запрос на сервер для обновления сообщения
            var messageId = messageElement.dataset.messageId; // Предполагается, что у вашего элемента сообщения есть атрибут данных messageId
            updateMessageOnServer(messageId, newText);
        }
    });
});

// Функция для отправки запроса на сервер для обновления сообщения
function updateMessageOnServer(messageId, newText) {
    console.log(messageId)
    $.ajax({
        url: '/Chat/UpdateMessage',
        method: 'POST',
        data: { messageId: messageId, newText: newText  },
        success: function (data) {
            // Обновляем страницу или выполняем другие действия после успешного выполнения запроса
            //location.reload();
        },
        error: function (xhr, status, error) {
            // Обрабатываем ошибку
            //console.error(error);
        }
    });
}