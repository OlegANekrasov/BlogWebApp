using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BlogWebApp.BLL.ViewModels;

namespace BlogWebApp.Controllers
{
    /// <summary>
    /// Default controller - handle incoming requests from the main page
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)   
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SuccessfullyRegistered()       
        {
            _logger.LogInformation("Успешная регистрация пользователя.");
            return View();
        }

        public IActionResult AccessIsDenied() 
        {
            _logger.LogError("Доступ запрещен.");
            return View();
        }

        [Route("/NotFound")]
        public IActionResult ResourceNotFound()
        {
            _logger.LogError("Страница не найдена.");
            return View();
        }

        public IActionResult SomethingWentWrong(string str)
        {
            if (str != null)
            {
                ViewBag.Message = str;

                if (!str.Contains("favicon"))
                {
                    _logger.LogError("Что-то пошло не так : " + str);
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}