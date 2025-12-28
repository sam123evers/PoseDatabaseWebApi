using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface ISessionService
    {
        Task<int> CreateSession(SessionModel sessionCreateObj);
    }
}
