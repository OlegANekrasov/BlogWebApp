using AutoMapper;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserManagementController> _logger;
        private readonly SignInManager<User> _signInManager;

        public static string PersonalData => "PersonalData";
        public static string About => "About";
        public static string ChangePassword => "ChangePassword";

        public UserManagementController(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork, ILogger<UserManagementController> logger, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> PersonalDataView()
        {
            var result = _userManager.GetUserAsync(User);
            var model = _mapper.Map<UserEditViewModel>(result.Result);

            return View("PersonalDataView", model);
        }

        [HttpGet]
        public ActionResult UploadImage(string id)
        {
            var model = new PhotoViewModel() { UserId = id };
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

                    var user = await _userManager.FindByIdAsync(model.UserId);

                    user.Image = memoryStream.ToArray();

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Некорректные данные");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("PersonalDataView");
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDataView(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("PersonalDataView");
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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
            var result = _userManager.GetUserAsync(User);
            var model = _mapper.Map<UserEditViewModel>(result.Result);

            return View("AboutView", model);
        }

        [HttpPost]
        public async Task<IActionResult> AboutView(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return View("AboutView", model);
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
