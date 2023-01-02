using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class BlogArticleController : Controller
    {
        public IBlogArticleService blogArticleService;

        public BlogArticleController(IBlogArticleService _blogArticleService)
        {
            blogArticleService = _blogArticleService;
        }

        public IActionResult Index()
        {
            var all_blogArticles = blogArticleService.GetAll();
            BlogArticleListViewModel model = new BlogArticleListViewModel(all_blogArticles);

            return View("Index", model);
        }
    }
}
