using AutoMapper;
using PoseDatabaseWebApi.Data.Dto.Identity.Users;
using PoseDatabaseWebApi.Models.Identity;

namespace PoseDatabaseWebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AspNetUserDto, AppUserModel>().ReverseMap();
            //CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
