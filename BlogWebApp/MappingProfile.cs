using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserEditViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));

            CreateMap<CreateBlogArticleViewModel, AddBlogArticle>();

            CreateMap<BlogArticle, EditBlogArticleViewModel>();

            CreateMap<EditBlogArticleViewModel, EditBlogArticle>();

            CreateMap<BlogArticle, DeleteBlogArticleViewModel>().ForMember(x => x.UserName, opt => opt.MapFrom(c => c.User.Email))
                                                                .ForMember(x => x.DateCreation, opt => opt.MapFrom(c => c.DateCreation.ToString("dd.MM.yyyy")));
            
            CreateMap<DeleteBlogArticleViewModel, DelBlogArticle>();
        }
    }
}
