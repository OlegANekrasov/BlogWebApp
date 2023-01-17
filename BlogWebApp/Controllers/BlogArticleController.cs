using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class BlogArticleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IBlogArticleService _blogArticleService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public BlogArticleController(UserManager<User> userManager, IBlogArticleService blogArticleService, IMapper mapper, ITagService tagService)
        {
            _userManager = userManager;
            _blogArticleService = blogArticleService;
            _mapper = mapper;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string sortOrder, string currentFilter, string searchString, string currentFilter1, string searchString1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var all_blogArticles = ((BlogArticleService)_blogArticleService).GetAllIncludeTags();

            if (searchString != null || searchString1 != null)
            {
                pageNumber = 1;
            }
            else
            {
                if (searchString == null)
                    searchString = currentFilter;
                
                if (searchString1 == null)
                    searchString1 = currentFilter1;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilter1"] = searchString1;

            if (!String.IsNullOrEmpty(searchString))
            {
                all_blogArticles = all_blogArticles.Where(s => s.User.Email.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(searchString1))
            {
                all_blogArticles = all_blogArticles.Where(s => s.Tags.FirstOrDefault(o => o.Name.ToUpper().Contains(searchString1.ToUpper())) != null).ToList();
            }

            ViewData["CurrentSort"] = sortOrder;
            all_blogArticles = ((BlogArticleService)_blogArticleService).SortOrder(all_blogArticles, sortOrder);
            
            var pageSize = 5;

            BlogArticleListViewModel blogArticleListViewModel = new BlogArticleListViewModel(all_blogArticles, user);
            var userQueryable = blogArticleListViewModel._blogArticles.AsQueryable();

            var model = PaginatedList<BlogArticleViewModel>.CreateAsync(userQueryable, pageNumber ?? 1, pageSize);

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var tags = ((TagService)_tagService).GetAll().OrderBy(o => o.Name).ToList();
            CreateBlogArticleViewModel model = new CreateBlogArticleViewModel(tags) { UserId = user.Id };

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogArticleViewModel incomingmModel)
        {
            var userId = incomingmModel.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{userId}'." });
            }
            else
            {
                var model = _mapper.Map<AddBlogArticle>(incomingmModel);
                model.Tags = incomingmModel.Tags.Where(o => o.IsTagSelected).Select(o => o.Name).ToList();
                
                await _blogArticleService.Add(model, user);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена статья с ID '{id}'." });
            }

            if(user != blogArticle.User && !User.IsInRole("Администратор") && !User.IsInRole("Модератор"))
            {
                return Redirect("~/Home/AccessIsDenied");
            }

            var tags = ((TagService)_tagService).GetAll().Select(o => o.Name).OrderBy(o => o).ToList();
            var model = _mapper.Map<EditBlogArticleViewModel>(blogArticle);
            
            model.SetTags(tags, blogArticle.Tags.Select(o => o.Name).OrderBy(o => o).ToList()); 
            model.UserId = user.Id;
            
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogArticleViewModel incomingmModel)
        {
            var model = _mapper.Map<EditBlogArticle>(incomingmModel);
            model.SetTags(incomingmModel.Tags.Where(o => o.IsTagSelected).ToList());

            await _blogArticleService.Edit(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена статья с ID '{id}'." });
            }

            if (user != blogArticle.User && !User.IsInRole("Администратор") && !User.IsInRole("Модератор"))
            {
                return Redirect("~/Home/AccessIsDenied");
            }

            var model = _mapper.Map<DeleteBlogArticleViewModel>(blogArticle);
            model.Tags = ((BlogArticleService)_blogArticleService).SetTagsInModel(blogArticle.Tags);

            return View("Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteBlogArticleViewModel incomingmModel)
        {
            var id = incomingmModel.Id;
            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена статья с ID '{id}'." });
            }

            var model = _mapper.Map<DelBlogArticle>(incomingmModel);
            await _blogArticleService.Delete(model);

            return RedirectToAction("Index");
        }
    }
}
