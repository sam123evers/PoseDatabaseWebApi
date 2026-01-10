using PoseDatabaseWebApi.Data.Dto.Identity;
using PoseDatabaseWebApi.Data.Dto.Pose;

namespace PoseDatabaseWebApi.Data.App
{
    public interface IPoseDataAccess
    {
        Task<List<PoseDto>> SelectPoseListAsync();
        Task<List<PoseDto>> SearchPosesAsync(string searchTerm);
        Task<int> InsertPoseAsync(PoseDto poseCreateObj);
        Task<int> UpdatePoseAsync(UpdatePoseDto poseUpdateObj);
        Task<int> SetDeletePoseAsync(int poseId);
    }
}
