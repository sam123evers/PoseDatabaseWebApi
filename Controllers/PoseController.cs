using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Data;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoseController : ControllerBase
    {
        private PoseDataContext _context;

        //private readonly ILogger<PoseController> _logger;

        public PoseController
        (
            PoseDataContext context
            //ILogger<PoseController> logger
        )
        {
            _context = context;
            //_logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pose>> GetAllPoses()
        {
            return Ok(_context.Poses.ToList());
        }
    }
}
