using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoseDatabaseWebApi.Models
{
    public interface IPoseRepository
    {
        Task<Pose> GetPose(int id);
        Task<IEnumerable<Pose>> GetPoses();
        Task<Pose> UpdatePoseDetails(int id, Pose pose);
        Task<Pose> AddPoseToDb(Pose pose);
        Task<Pose> DeletePose(int id);
    }
}
