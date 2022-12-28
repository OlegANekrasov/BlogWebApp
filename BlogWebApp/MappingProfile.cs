using AutoMapper;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models.Users;

namespace BlogWebApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserEditViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));
        }
    }
}
