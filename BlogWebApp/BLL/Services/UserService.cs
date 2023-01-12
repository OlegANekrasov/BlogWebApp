using AutoMapper;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly IRepository<Comment> _commentsRepository;

        public UserService(UserManager<User> userManager, IMapper mapper, IRepository<BlogArticle> blogArticlesRepository, IRepository<Comment> commentsRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _blogArticlesRepository = blogArticlesRepository;
            _commentsRepository = commentsRepository;
        }
        
        public async Task<UserViewModel> GetUserViewModel(User user)
        {
            var model = _mapper.Map<UserViewModel>(user);

            var blogArticlesCount = ((BlogArticlesRepository)_blogArticlesRepository).GetAllByUserId(user.Id).Count().ToString();
            var commentsCount = ((CommentsRepository)_commentsRepository).GetAllByUserId(user.Id).Count().ToString();

            model.BlogArticles = $"Статей ({blogArticlesCount})   Комментариев ({commentsCount})";
            
            List<UserRole> listRoles = new List<UserRole>();
            
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles.OrderBy(o => o))
            {
                listRoles.Add(new UserRole() { Name = item, IsRoleAssigned = false });
            }

            model.Roles = listRoles;

            return model;
        }
    }
}
