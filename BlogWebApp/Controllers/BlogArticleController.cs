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

        public BlogArticleController(UserManager<User> userManager, IBlogArticleService blogArticleService, IMapper mapper)
        {
            _userManager = userManager;
            _blogArticleService = blogArticleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int? pageNumber, string sortOrder, string currentFilter, string searchString, string currentFilter1, string searchString1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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

            switch (sortOrder)
            {
                case "Title":
                    all_blogArticles = all_blogArticles.OrderBy(s => s.Title).ToList();
                    break;
                case "Author":
                    all_blogArticles = all_blogArticles.OrderBy(s => s.User?.Email).ToList();
                    break;
                case "DateCreation":
                    all_blogArticles = all_blogArticles.OrderByDescending(s => s.DateCreation).ToList();
                    break;
                default:
                    all_blogArticles = all_blogArticles.OrderByDescending(s => s.DateCreation).ToList();
                    break;
            }

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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CreateBlogArticleViewModel model = new CreateBlogArticleViewModel() { UserId = user.Id };

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogArticleViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(incomingmModel.UserId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
                else
                {
                    var model = _mapper.Map<AddBlogArticle>(incomingmModel);
                    await _blogArticleService.Add(model, user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            if(user != blogArticle.User)
            {
                return NotFound("Страница не доступна.");
            }

            var model = _mapper.Map<EditBlogArticleViewModel>(blogArticle);
            model.Tags = ((BlogArticleService)_blogArticleService).SetTagsInModel(blogArticle.Tags);

            model.UserId = user.Id;
            
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogArticleViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<EditBlogArticle>(incomingmModel);
                await _blogArticleService.Edit(model);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            if (user != blogArticle.User)
            {
                return NotFound("Страница не доступна.");
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
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            var model = _mapper.Map<DelBlogArticle>(incomingmModel);
            await _blogArticleService.Delete(model);

            return RedirectToAction("Index");
        }
    }
}
