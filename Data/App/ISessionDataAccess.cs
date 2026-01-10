using PoseDatabaseWebApi.Data.Dto.Session;

namespace PoseDatabaseWebApi.Data.App
{
    public interface ISessionDataAccess
    {
        Task<List<SessionDto>> SelectAllSessionsAndSequencesAsync();
        Task<List<SessionDto>> SelectMySessionsAndSequencesAsync(string userId);
        Task<int> InsertSessionAsync(SessionDto sessionCreateObj, string userId);
        Task<int> UpdateSessionAsync(SessionDto seshDto);
    }
}
