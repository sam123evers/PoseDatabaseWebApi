using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface IPoseWebService
    {
        //#region Users
        //Task<List<UserDataModel>> GetUserData();

        //Task<int> CreateUser(UserDataModel userCreateObj);

        //Task<int> UpdateUser(UpdateUserDataModel userUpdateeObj);

        //Task<int> SetDeleteUser(int userDataId);
        //#endregion

        #region Poses
        Task<List<PoseModel>> GetPoseList();
        Task<int> CreatePose(PoseModel poseCreateObj);
        Task<int> UpdatePose(UpdatePoseModel poseUpdateObj);
        Task<int> SetDeletePose(int poseId);
        #endregion
    }
}
