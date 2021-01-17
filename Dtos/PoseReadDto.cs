using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseDatabaseWebApi.Dtos
{
    public class PoseReadDto
    {
        public int Id { get; set; }
        public string PoseName { get; set; }
        public string PoseOriginName { get; set; }
        public string PoseOriginStyle { get; set; }

    }
}
