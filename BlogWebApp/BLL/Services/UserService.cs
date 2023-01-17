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

        public async Task<List<UserListModel>> CreateUserListModel(IQueryable<User> users)
        {
            var userList = new List<UserListModel>();
            foreach (var user in users)
            {
                string Id = user.Id;
                string? userName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                string? email = user.Email;
                byte[]? image = user.Image;

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    string? roleName = "";
                    bool first = true;
                    foreach (var role in userRoles.OrderBy(o => o))
                    {
                        if (first)
                        {
                            roleName = role;
                            first = false;
                        }
                        else
                        {
                            roleName += (", " + role);
                        }
                    }

                    userList.Add(new UserListModel(Id, userName, email, roleName, image));
                }
            }

            return userList;
        }

        public List<UserListModel> SortOrder(List<UserListModel> userList, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Username":
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
                case "Email":
                    userList = userList.OrderBy(s => s.Email).ToList();
                    break;
                case "RoleName":
                    userList = userList.OrderBy(s => s.RoleName).ToList();
                    break;
                default:
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
            }
            return userList;
        }
    }
}
