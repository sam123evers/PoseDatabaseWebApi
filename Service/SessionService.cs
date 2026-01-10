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
        public async Task<int> CreateSession(SessionModel sessionCreateObj, string userId)
        {
            return await _sessionDataAccess.InsertSessionAsync(_mapper.Map<SessionDto>(sessionCreateObj), userId);
        }

        public async Task<int> UpdateSession(SessionModel seshUpdateObj)
        {
            return await _sessionDataAccess.UpdateSessionAsync(_mapper.Map<SessionDto>(seshUpdateObj));
        }

        public async Task<List<SessionModel>> GetMySessionListWithSequences(string userId)
        {
            var sessionDtoList = await _sessionDataAccess.SelectMySessionsAndSequencesAsync(userId);
            return _mapper.Map<List<SessionModel>>(sessionDtoList);
        }

        public async Task<List<SessionModel>> GetAllSessionListWithSequences()
        {
            var sessionDtoList = await _sessionDataAccess.SelectAllSessionsAndSequencesAsync();
            return _mapper.Map<List<SessionModel>>(sessionDtoList);
        }
    }
}
