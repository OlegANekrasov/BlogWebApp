using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
