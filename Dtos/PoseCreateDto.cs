using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseDatabaseWebApi.Dtos
{
    public class PoseCreateDto
    {
        public string PoseName { get; set; }
        public string PoseOriginName { get; set; }
        public string PoseOriginStyle { get; set; }

    }
}
