namespace PoseDatabaseWebApi.Service
{
    public interface IUserService
    {
        Task<bool> CreateNewUserAsync(string username, string password, string email);
    }
}
