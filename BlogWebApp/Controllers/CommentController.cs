using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
