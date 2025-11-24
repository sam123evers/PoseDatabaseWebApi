using AutoMapper;
using PoseDatabaseWebApi.Data.Dto;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Create mapping between UserDto and UserDataModel
            CreateMap<UserDto, UserDataModel>().ReverseMap();
            CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
