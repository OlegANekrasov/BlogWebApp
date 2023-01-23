using AutoMapper;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Extentions;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.IO.Compression;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserManagementController> _logger;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public static string PersonalData => "PersonalData";
        public static string About => "About";
        public static string ChangePassword => "ChangePassword";

        public UserManagementController(UserManager<User> userManager, 
                                        IMapper mapper, 
                                        ILogger<UserManagementController> logger,
                                        IUserService userService,
                                        SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> PersonalDataView(string id = null)
        {
            User user;
            if(id == null)
            {
                user = await ((UserService)_userService).GetUserAsync(User);
            }
            else
            {
                user = await ((UserService)_userService).FindByIdAsync(id);
            }

            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка получения данных пользователя из БД." });
            }

            var model = _mapper.Map<UserEditViewModel>(user);

            return View("PersonalDataView", model);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDataView(UserEditViewModel model)
        {
            var user = await ((UserService)_userService).FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка получения данных пользователя из БД." });
            }

            user.Convert(model);

            if (!await _userService.UpdateAsync(user, model.Email))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка обновления данных пользователя '{model.Email}'." });
            }

            return RedirectToAction("PersonalDataView");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserView(string id)
        {
            var user = await ((UserService)_userService).FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка получения данных пользователя из БД." });
            }

            var model = _mapper.Map<UserEditViewModel>(user);

            return View("EditUserView", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserView(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            user.Convert(model);

            if (!await _userService.UpdateAsync(user, model.Email))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка обновления данных пользователя '{model.Email}'." });
            }

            return RedirectToAction("EditUserView");
        }

        [HttpGet]
        public ActionResult UploadImage(string id, string edit = null)
        {
            var model = new PhotoViewModel() { UserId = id };

            if(edit != null)
            {
                model.IsEditUserView = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UploadImage(PhotoViewModel model, IFormFile uploadImage)          
        {
            if (uploadImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await uploadImage.CopyToAsync(memoryStream);

                    var user = await ((UserService)_userService).FindByIdAsync(model.UserId);
                    if (user == null)
                    {
                        return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка получения данных пользователя из БД." });
                    }

                    user.Image = memoryStream.ToArray();

                    if (!await _userService.UpdateAsync(user, user.Email))
                    {
                        return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка обновления данных пользователя '{user.Email}'." });
                    }
                }
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка загрузки файла." });
            }

            if(model.IsEditUserView)
            {
                return RedirectToAction("EditUserView", new {id = model.UserId });
            }
            else
            {
                return RedirectToAction("PersonalDataView");
            }
        }

        [HttpGet]
        public IActionResult ChangePasswordView()
        {
            var model = new ChangePasswordViewModel();
            return View("ChangePasswordView", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordView(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await ((UserService)_userService).GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User changed their password successfully.");
            }

            return View("ChangePasswordView", model);
        }

        [HttpGet]
        public async Task<IActionResult> AboutView()
        {
            var user = await ((UserService)_userService).GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var model = _mapper.Map<UserEditViewModel>(user);
            return View("AboutView", model);
        }

        [HttpPost]
        public async Task<IActionResult> AboutView(UserEditViewModel model)
        {
            var user = await ((UserService)_userService).FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            user.Convert(model);

            if (!await _userService.UpdateAsync(user, user.Email))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка обновления данных пользователя '{user.Email}'." });
            }

            return View("AboutView", model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowUser(string userId)
        {
            var user = await ((UserService)_userService).FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var model = await _userService.GetUserViewModelAsync(user);
            return View("UserView", model);
        }

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);
        public static string AboutNavClass(ViewContext viewContext) => PageNavClass(viewContext, About);
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
