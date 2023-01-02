using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
