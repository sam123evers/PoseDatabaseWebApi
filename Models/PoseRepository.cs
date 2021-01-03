using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PoseDatabaseWebApi.Models
{
    public class PoseRepository : IPoseRepository
    {
        private readonly PoseContext _context;

        public PoseRepository(PoseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pose>> GetPoses()
        {
            IEnumerable<Pose> result = await _context.Poses.ToListAsync();
            return result;
        }

        public async Task<Pose> GetPose(int id)
        {
            Pose pose = await _context.Poses.FindAsync(id);
            if (pose == null)
            {
                throw new ArgumentException(
                    $"The Pose Database does not contain a pose with that ID");
            }
            else
            {
                return pose;
            }
        }


        public async Task<Pose> AddPoseToDb(Pose pose)
        {
            _context.Poses.Add(pose);
            await _context.SaveChangesAsync();
            var result = await _context.Poses.FindAsync(pose.Id);

            return result;
        }

        public async Task<Pose> DeletePose(int id)
        {
            Pose pose = await _context.Poses.FindAsync(id);

            _context.Poses.Remove(pose);
            await _context.SaveChangesAsync();

            return pose;
        }

        public async Task<Pose> UpdatePoseDetails(int id, Pose pose)
        {
            var poseFromDb = new Pose();
            try
            {
                poseFromDb = _context.Poses.FirstOrDefault(x => x.Id == id);

                if (poseFromDb != null)
                {
                    poseFromDb.PoseOriginStyle = pose.PoseOriginStyle;
                    poseFromDb.PoseName = pose.PoseName;
                    poseFromDb.PoseOriginName = pose.PoseOriginName;
                    await _context.SaveChangesAsync();
                }

            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!PoseExists(id))
                {
                    throw new ArgumentException(e.Message);
                }
            }
            return poseFromDb;
        }

        private bool PoseExists(int id)
        {
            return _context.Poses.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<Pose>> Search(string input)
        {
            IEnumerable<Pose> result = _context.Poses;

            if (!string.IsNullOrEmpty(input))
            {
                result = result.Where(e => e.PoseName.Contains(input)
                    || e.PoseOriginName.Contains(input));
            }

            return result;
        }
    }
}
