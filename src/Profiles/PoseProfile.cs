using AutoMapper;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Dtos;

namespace PoseDatabaseWebApi.Profiles
{
    public class PoseProfile : Profile
    {
        public PoseProfile()
        {
            // Flow of Data:
            // Source --> Target
            CreateMap<Pose, PoseReadDto>();

            CreateMap<PoseCreateDto, Pose>();

            CreateMap<PoseUpdateDto, Pose>();

            CreateMap<Pose, PoseUpdateDto>();
        }
    }
}
