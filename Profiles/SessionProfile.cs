using AutoMapper;
using PoseDatabaseWebApi.Data.Dto.Session;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Profiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<SessionDto, SessionModel>().ReverseMap();
        }
    }
}
