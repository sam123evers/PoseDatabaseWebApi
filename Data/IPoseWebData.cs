using PoseDatabaseWebApi.Data.Dto;

namespace PoseDatabaseWebApi.Data
{
    public interface IPoseWebData
    {
        Task<List<UserDto>> GetUsersAsync();

        Task<int> CreateUserAsync(UserDto userCreateObj);

        Task<int> UpdateUserAsync(UpdateUserDto userUpdateObj);

        Task<int> SetDeleteUserAsync(int userDataId);
    }
}
