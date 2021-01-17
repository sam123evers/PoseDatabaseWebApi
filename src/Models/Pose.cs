using Microsoft.AspNetCore.Mvc;
using System;

namespace PoseDatabaseWebApi.Models
{
    public class Pose
    {
        public int Id { get; set; }
        public string PoseName { get; set; }

        public string PoseOriginName { get; set; }

        public string PoseOriginStyle { get; set; }

        public static implicit operator Pose(ActionResult<Pose> v)
        {
            throw new NotImplementedException();
        }
    }
}
