using AutoMapper;
using PoseDatabaseWebApi.Data.Dto.Sequence;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class SequenceProfile : Profile
    {
        public SequenceProfile()
        {
            CreateMap<SequenceDto, SequenceModel>().ReverseMap();
            CreateMap<SequencePoseDto, SequencePoseModel>().ReverseMap();
        }
    }
}
