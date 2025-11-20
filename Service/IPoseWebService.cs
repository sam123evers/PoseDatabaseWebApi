using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface IPoseWebService
    {
       Task<List<UserDataModel>> GetUserData();
    }
}
