using AutoMapper;
using PoseDatabaseWebApi.Data.Dto;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class PoseProfile : Profile
    {
        public PoseProfile()
        {
            CreateMap<PoseDto, PoseModel>().ReverseMap();
            CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
