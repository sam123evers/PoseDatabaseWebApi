using AutoMapper;
using PoseDatabaseWebApi.Data.Dto.Identity;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class PoseProfile : Profile
    {
        public PoseProfile()
        {
            CreateMap<PoseDto, PoseModel>().ReverseMap();
            //CreateMap<UpdateUserDto, UpdateUserDataModel>().ReverseMap();
        }
    }
}
