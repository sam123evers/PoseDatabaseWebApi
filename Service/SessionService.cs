using AutoMapper;
using PoseDatabaseWebApi.Data.App;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Data.Dto.Session;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public class SessionService : ISessionService
    {
        private readonly ISessionDataAccess _sessionDataAccess;
        private readonly IMapper _mapper;
        public SessionService(ISessionDataAccess sessionData, IMapper mapper)
        {
            _sessionDataAccess = sessionData;
            _mapper = mapper;
        }
        public async Task<int> CreateSession(SessionModel sessionCreateObj)
        {
            return await _sessionDataAccess.InsertSessionAsync(_mapper.Map<SessionDto>(sessionCreateObj));
        }
    }
}
