using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoseController : ControllerBase
    {
        Pose pose_hs = new Pose()
        {
            PoseName = "Hand Stand"
        };

        private readonly ILogger<PoseController> _logger;

        public PoseController
        (
            //ILogger<PoseController> logger
        )
        {
            //_logger = logger;
        }

        [HttpGet]
        public Pose Get()
        {
            return pose_hs;
        }
    }
}
