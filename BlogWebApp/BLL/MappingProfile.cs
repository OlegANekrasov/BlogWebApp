using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserEditViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));
            CreateMap<User, UserViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));

            CreateMap<ApplicationRole, ShowRoleViewModel>();

            CreateMap<CreateBlogArticleViewModel, AddBlogArticle>();

            CreateMap<BlogArticle, EditBlogArticleViewModel>().ForMember(x => x.Tags, opt => opt.Ignore());
            CreateMap<EditBlogArticleViewModel, EditBlogArticle>().ForMember(x => x.Tags, opt => opt.Ignore());

            CreateMap<DeleteBlogArticleViewModel, DelBlogArticle>();
            CreateMap<BlogArticle, DeleteBlogArticleViewModel>().ForMember(x => x.UserName, opt => opt.MapFrom(c => c.User.Email))
                                                                .ForMember(x => x.DateCreation, opt => opt.MapFrom(c => c.DateCreation.ToString("dd.MM.yyyy")));

            CreateMap<CreateTagViewModel, AddTag>();
            CreateMap<DeleteTagViewModel, DelTag>();
            CreateMap<EditTagViewModel, EditTag>();

            CreateMap<EditTag, AddTag>();
            CreateMap<EditTag, DelTag>();

            CreateMap<Comment, EditCommentViewModel>();
            CreateMap<CreateCommentViewModel, AddComment>();
            CreateMap<EditCommentViewModel, EditComment>();
            CreateMap<DeleteCommentViewModel, DelComment>();

            CreateMap<DeletePhotoBlogArticleViewModel, DelBlogArticleImage>();
        }
    }
}
