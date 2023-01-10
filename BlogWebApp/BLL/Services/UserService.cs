using AutoMapper;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly IRepository<Comment> _commentsRepository;

        public UserService(IMapper mapper, IRepository<BlogArticle> blogArticlesRepository, IRepository<Comment> commentsRepository)
        {
            _mapper = mapper;
            _blogArticlesRepository = blogArticlesRepository;
            _commentsRepository = commentsRepository;
        }
        
        public UserViewModel GetUserViewModel(User user)
        {
            var model = _mapper.Map<UserViewModel>(user);

            var blogArticlesCount = ((BlogArticlesRepository)_blogArticlesRepository).GetAllByUserId(user.Id).Count().ToString();
            var commentsCount = ((CommentsRepository)_commentsRepository).GetAllByUserId(user.Id).Count().ToString();

            model.BlogArticles = $"Статей ({blogArticlesCount})   Комментариев ({commentsCount})";
            return model;
        }
    }
}
