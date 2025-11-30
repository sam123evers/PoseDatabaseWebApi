using AutoMapper;
using PoseDatabaseWebApi.Data.Dto.Identity.User;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AspNetUserDto, UserDataModel>().ReverseMap();
            //CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
