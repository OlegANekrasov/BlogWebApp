using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogWebApp.Tests.Controllers
{
    public class TagControllerTests
    {
        [Fact]
        public async Task IndexActionModelIsComplete()
        {
            // Arrange - устанавливает начальные условия для выполнения теста
            var user = GetUser();

            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetListTagsViewModel(user)).Returns(GetTestListTags());

            var identity = new GenericIdentity("some name", "test");
            var contextUser = new ClaimsPrincipal(identity);

            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(m => m.GetUserAsync(contextUser).Result).Returns(user);
            
            var controller = new TagController(mock.Object, fakeUserManager.Object, null, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = contextUser }
            };
            
            // Act - выполняет тест (обычно представляет одну строку кода)
            var result = await controller.Index();

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListTagsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model._tags.Count());
        }

        private User GetUser()
        {
            return new User() { Id = Guid.NewGuid().ToString(), Email = "Test@mail.ru" };
        }

        private ListTagsViewModel GetTestListTags()
        {
            var listTagsViewModel = new ListTagsViewModel();
            listTagsViewModel._tags = new List<TagViewModel>();
            
            var tagViewModel = new TagViewModel(Guid.NewGuid().ToString(), "Книги", "2");
            listTagsViewModel._tags.Add(tagViewModel);

            tagViewModel = new TagViewModel(Guid.NewGuid().ToString(), "Отдых", "3");
            listTagsViewModel._tags.Add(tagViewModel);

            tagViewModel = new TagViewModel(Guid.NewGuid().ToString(), "Разное", "5");
            listTagsViewModel._tags.Add(tagViewModel);

            return listTagsViewModel;
        }

        [Fact]
        public void CreateActionModelIsComplete()
        {
            // Arrange - устанавливает начальные условия для выполнения теста
            var controller = new TagController(null, null, null, null);

            // Act - выполняет тест (обычно представляет одну строку кода)
            var result = controller.Create();

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequestResult_WhenModelStateIsInvalid_EmptyName()
        {
            // Arrange
            var controller = new TagController(null, null, null, null);
            controller.ModelState.AddModelError("Name", "Required");
            var createTagViewModel = new CreateTagViewModel();

            // Act
            var result = await controller.Create(createTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequestResult_WhenModelStateIsInvalid_FewWordsInName()
        {
            // Arrange
            var controller = new TagController(null, null, null, null);
            controller.ModelState.AddModelError("Name", "TagName");
            
            var createTagViewModel = new CreateTagViewModel() { Name = "Few Words" };

            // Act
            var result = await controller.Create(createTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequestResult_WhenThisNameAlreadyExists()
        {
            // Arrange
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetAll()).Returns(GetTags());

            var controller = new TagController(mock.Object, null, null, null);
            controller.ModelState.Clear();

            var createTagViewModel = new CreateTagViewModel() { Name = "Разное" };

            // Act
            var result = await controller.Create(createTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateTagViewModel>(viewResult.ViewData.Model);
        }

        private IEnumerable<Tag> GetTags()
        {
            yield return new Tag { Name = "Разное" };
        }

        [Fact]
        public async Task CreatePost_ReturnsRedirectAndAddsTag_WhenModelStateIsValid()
        {
            // Arrange
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetAll()).Returns(GetTags());
            mock.Setup(m => m.Add(new AddTag())).Returns(Task.CompletedTask);

            var mockMapper = new Mock<IMapper>();
            var mockLoger = new Mock<ILogger<TagController>>();

            var controller = new TagController(mock.Object, null, mockMapper.Object, mockLoger.Object);
            controller.ModelState.Clear();

            var createTagViewModel = new CreateTagViewModel() { Name = "Что-то" };

            // Act
            var result = await controller.Create(createTagViewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsRedirect_WhenTagNotFoundById()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();

            mock.Setup(m => m.Get(id)).Returns((Tag)null);

            var controller = new TagController(mock.Object, null, null, null);

            // Act
            var result = controller.Edit(id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("SomethingWentWrong", redirectToActionResult.ActionName);
        }

        [Fact]
        public void EditActionModelIsComplete()
        {
            // Arrange - устанавливает начальные условия для выполнения теста
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();

            mock.Setup(m => m.Get(id)).Returns(GetTag());

            var controller = new TagController(mock.Object, null, null, null);

            // Act
            var result = controller.Edit(id);

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditTagViewModel>(viewResult.ViewData.Model);
        }

        private Tag GetTag()
        {
            return new Tag { Name = "Разное" };
        }

        [Fact]
        public async Task EditPost_ReturnsBadRequestResult_WhenModelStateIsInvalid_EmptyName()
        {
            // Arrange
            var controller = new TagController(null, null, null, null);
            controller.ModelState.AddModelError("Name", "Required");
            var editTagViewModel = new EditTagViewModel();

            // Act
            var result = await controller.Edit(editTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsBadRequestResult_WhenModelStateIsInvalid_FewWordsInName()
        {
            // Arrange
            var controller = new TagController(null, null, null, null);
            controller.ModelState.AddModelError("Name", "TagName");

            var editTagViewModel = new EditTagViewModel() { Name = "Few Words" };

            // Act
            var result = await controller.Edit(editTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsBadRequestResult_WhenThisNameAlreadyExists()
        {
            // Arrange
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetAll()).Returns(GetTags());

            var controller = new TagController(mock.Object, null, null, null);
            controller.ModelState.Clear();

            var editTagViewModel = new EditTagViewModel() { Id = "123", Name = "Разное" };

            // Act
            var result = await controller.Edit(editTagViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsRedirectAndAddsTag_WhenModelStateIsValid()
        {
            // Arrange
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetAll()).Returns(GetTags());

            var mockMapper = new Mock<IMapper>();
            var mockLoger = new Mock<ILogger<TagController>>();

            var controller = new TagController(mock.Object, null, mockMapper.Object, mockLoger.Object);
            controller.ModelState.Clear();

            var editTagViewModel = new EditTagViewModel() { Id = "123", Name = "Что-то" };

            // Act
            var result = await controller.Edit(editTagViewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsRedirect_WhenTagNotFoundById()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();

            mock.Setup(m => m.Get(id)).Returns((Tag)null);

            var controller = new TagController(mock.Object, null, null, null);

            // Act
            var result = controller.Delete(id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("SomethingWentWrong", redirectToActionResult.ActionName);
        }

        [Fact]
        public void DeleteActionModelIsComplete()
        {
            // Arrange - устанавливает начальные условия для выполнения теста
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();

            mock.Setup(m => m.Get(id)).Returns(GetTag());

            var controller = new TagController(mock.Object, null, null, null);

            // Act
            var result = controller.Delete(id);

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DeleteTagViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void DeletePost_ReturnsRedirect_WhenTagNotFoundById()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();

            mock.Setup(m => m.Get(id)).Returns((Tag)null);

            var controller = new TagController(mock.Object, null, null, null);

            // Act
            var result = controller.Delete(id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("SomethingWentWrong", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeletePost_ReturnsRedirectAndDelsTag()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.Get(id)).Returns(GetTag()).Verifiable();

            var mockMapper = new Mock<IMapper>();
            var mockLoger = new Mock<ILogger<TagController>>();

            var controller = new TagController(mock.Object, null, mockMapper.Object, mockLoger.Object);
            controller.ModelState.Clear();

            var deleteTagViewModel = new DeleteTagViewModel() { Id = id, Name = "Что-то" };

            // Act
            var result = await controller.Delete(deleteTagViewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify();
        }
    }
}
