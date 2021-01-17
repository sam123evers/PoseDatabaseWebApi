using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoseDatabaseWebApi.Models
{
    public interface IPoseRepository
    {
        bool SaveChanges();
        IEnumerable<Pose> Search(string input);
        Pose GetPose(int id);
        IEnumerable<Pose> GetPoses();
        Pose UpdatePoseDetails(int id, Pose pose);
        void PatchPoseDetails(Pose pose);
        void AddPoseToDb(Pose pose);
        void DeletePose(Pose pose);
    }
}
