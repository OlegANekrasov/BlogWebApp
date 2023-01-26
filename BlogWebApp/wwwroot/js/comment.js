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



    /*

                        '@if (item.Image != null && item.Image.Length > 0)' +
                        '{' +
                            '@Html.Raw("<img style=\'height:120px;\' src=\"data:image/jpeg;base64," + Convert.ToBase64String(item.Image) + "\" />")' +
                        '}' +

                        '@if (ViewBag.UserEmail == item.Author || (await AuthorizationService.AuthorizeAsync(User, "RequireAdministratorRole")).Succeeded' +
                                                              || (await AuthorizationService.AuthorizeAsync(User, "RequireModeratorRole")).Succeeded)\
                        {\
                            <a asp-action="Edit" asp-route-id="@item.Id" asp-route-blogArticleId="@Model.blogArticle.Id" class="btn btn-sm btn-outline-secondary"> Редактировать </a>\
                            <a asp-action="Delete" asp-route-id="@item.Id" asp-route-blogArticleId="@Model.blogArticle.Id" class="btn btn-sm btn-outline-danger"> Удалить </a>\
                        }\
                        <a href="/UserManagement/ShowUser?userId=f78b3e55-52d5-4758-8372-b9d7b7f0d5a8"> olegnekrasov@live.com </a>
    */
    


    document.getElementById('notify').innerHTML += html;  

});
hubConnection.start();