using AutoMapper;
using PoseDatabaseWebApi.Data.Dto;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserDataModel>().ReverseMap();
            CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
