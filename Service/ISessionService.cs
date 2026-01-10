using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface ISessionService
    {
        Task<int> CreateSession(SessionModel sessionCreateObj, string userId);

        Task<int> UpdateSession(SessionModel sessionCreateObj);

        Task<List<SessionModel>> GetMySessionListWithSequences(string userId);

        Task<List<SessionModel>> GetAllSessionListWithSequences();
    }
}
