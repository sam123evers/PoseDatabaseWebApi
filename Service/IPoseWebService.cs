using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface IPoseWebService
    {
        Task<List<PoseModel>> GetPoseList();
        Task<List<PoseModel>> SearchPoses(string searchTerm);
        Task<int> CreatePose(PoseModel poseCreateObj);
        Task<int> UpdatePose(UpdatePoseModel poseUpdateObj);
        Task<int> SetDeletePose(int poseId);
    }
}
