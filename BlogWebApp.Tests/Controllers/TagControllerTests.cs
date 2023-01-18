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
using System.Text;
using System.Threading.Tasks;

namespace BlogWebApp.Tests.Controllers
{
    public class TagControllerTests
    {
        [Fact]
        public async Task IndexActionModelIsComplete()
        {
            // Arrange - устанавливает начальные условия для выполнения теста
            var mock = new Mock<ITagService>();
            mock.Setup(m => m.GetListTagsViewModel()).Returns(GetTestListTags());

            var fakeUserManager = new Mock<FakeUserManager>();
            var controller = new TagController(mock.Object, fakeUserManager.Object, null);
             
            // Act - выполняет тест (обычно представляет одну строку кода)
            var result = await controller.Index();

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListTagsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model._tags.Count());
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
            var mock = new Mock<ITagService>();
            var fakeUserManager = new Mock<FakeUserManager>();
            var controller = new TagController(mock.Object, fakeUserManager.Object, null);

            // Act - выполняет тест (обычно представляет одну строку кода)
            var result = controller.Create();

            // Assert - верифицирует результат теста
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateTagViewModel>(viewResult.ViewData.Model);
        }
    }

    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(
                  new Mock<IUserStore<User>>().Object,
                  new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
        { }
    }
}
