using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    public class BlogArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
