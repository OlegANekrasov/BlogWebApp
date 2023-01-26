const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/blogHub")
    .build();

// получение сообщения от сервера
hubConnection.on('NewMessage', function (content, userId, blogArticleId, user, date, image) {
    
    if (blogArticleId != document.getElementById('BlogArticleId').value)
        return;

    var html = '';
    if (image != null) {
        html = '<div class="card" style="width: 36rem;">' +
                    '<div class="card-body">' + 
                        '<img style=\'height:120px;\' src="data:image/jpeg;base64,' + image + '"/>' +
                        '<p class="card-text">' + content + '</p>' +
                        '<a href="/UserManagement/ShowUser?userId=' + userId + '">' + user + ' </a>' +
                        date +
                    '</div>' +
                '</div> ';
    }
    else
    {
        html = '<div class="card" style="width: 36rem;">' +
            '<div class="card-body">' +
                        '<p class="card-text">' + content + '</p>' +
                        '<a href="/UserManagement/ShowUser?userId=' + userId + '">' + user + ' </a>' +
                        date +
                    '</div>' +
               '</div> ';
    }

    document.getElementById('notify').innerHTML += html;  
});

hubConnection.start();