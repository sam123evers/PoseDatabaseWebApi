using PoseDatabaseWebApi.Data.Dto;

namespace PoseDatabaseWebApi.Data
{
    public interface IPoseWebData
    {
        #region Users
        Task<List<UserDto>> GetUsersAsync();

        Task<int> CreateUserAsync(UserDto userCreateObj);

        Task<int> UpdateUserAsync(UpdateUserDto userUpdateObj);

        Task<int> SetDeleteUserAsync(int userDataId);

        #endregion

        #region Poses
        Task<List<PoseDto>> SelectPoseListAsync();
        Task<int> InsertPoseAsync(PoseDto poseCreateObj);
        Task<int> UpdatePoseAsync(UpdatePoseDto poseUpdateObj);
        Task<int> SetDeletePoseAsync(int poseId);

        #endregion
    }
}
