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

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Pose> GetPoses()
        {
            IEnumerable<Pose> result = _context.Poses.ToList();
            return result;
        }

        public Pose GetPose(int id)
        {
            var pose =  _context.Poses.FirstOrDefault(p => p.Id == id);
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

        public void AddPoseToDb(Pose pose)
        {
            if (pose == null)
            {
                throw new ArgumentNullException(nameof(pose));
            }

            _context.Poses.Add(pose);
        }

        public void DeletePose(Pose pose)
        {
            if (pose == null)
            {
                throw new ArgumentNullException(nameof(pose));
            }

            _context.Poses.Remove(pose);
        }

        public Pose UpdatePoseDetails(int id, Pose pose)
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
                    _context.SaveChanges();
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

        public void PatchPoseDetails(Pose pose)
        { 
            // Here...
            // We do nothing...s
        }

        private bool PoseExists(int id)
        {
            return _context.Poses.Any(e => e.Id == id);
        }

        public IEnumerable<Pose> Search(string input)
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
