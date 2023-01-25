const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/blogHub")
    .build();

// получение сообщения от сервера
hubConnection.on('NewMessage', function (content, userId, blogArticleId) {
    
    if (blogArticleId != document.getElementById('BlogArticleId').value)
        return;

    alert(document.getElementById('BlogArticleId').value);

    /*
    let elemCard = document.createElement("div");
    elemCard.classList.add('card');
    elemCard.style.cssText += 'width: 36rem;';

    let elemCardBody = document.createElement("div");
    elemCardBody.classList.add('card-body');

    let elemP = document.createElement("p");
    elemCard.classList.add('card-text');
    elemP.appendChild(document.createTextNode(content));

    let elemA = document.createElement("a");
    elemA.appendChild(document.createTextNode(userId));

    elemCardBody.appendChild(elemP);
    elemCardBody.appendChild(elemA);
    elemCard.appendChild(elemCardBody);

    document.getElementById("notify").appendChild(elem);

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
    

    var html = '<div class="card" style="width: 36rem;">' +
                    '<div class="card-body">' + 
                        '<p class="card-text">' + content + '</p>' +
                        '<a href="/UserManagement/ShowUser?userId=' + userId + '">' + userId + '</a>' +
                        " 25.01.2023" +
                    '</div>' +
                '</div> ';

    document.getElementById('notify').innerHTML += html;  

});
hubConnection.start();