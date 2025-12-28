using PoseDatabaseWebApi.Data.Dto.Session;

namespace PoseDatabaseWebApi.Data.App
{
    public interface ISessionDataAccess
    {
        Task<int> InsertSessionAsync(SessionDto sessionCreateObj);
    }
}
