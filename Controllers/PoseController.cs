using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.CustomActionResults;

namespace PoseDatabaseWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosesController : ControllerBase
    {
        private readonly IPoseRepository _poseRepository;

        public PosesController(IPoseRepository poseRepository)
        {
            _poseRepository = poseRepository;
        }

        // GET: api/Poses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pose>>> GetPoses()
        {
            var result = await _poseRepository.GetPoses();
            return Ok(result);
        }

        // GET: api/Poses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPose(int id)
        {
            try
            {
                var result = await _poseRepository.GetPose(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new ActionResultException(e);
            }
        }

        //[HttpGet("search/{searchTerm}")]
        //public async Task<ActionResult<IEnumerable<Pose>>> GetPosesWithSearchTerm(string searchTerm)
        //{
        //    IEnumerable<Pose> posesMatchingTerm = _context.Poses.Where(x => x.PoseName.Contains(searchTerm));

        //    if (posesMatchingTerm == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(posesMatchingTerm);
        //}

        // PUT: api/Poses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoseDetails(int id, Pose pose)
        {
            try
            {
                var updatedPose = await _poseRepository.UpdatePoseDetails(id, pose);
                return Ok(updatedPose);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //POST: api/Poses
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pose>> AddPoseToDb(Pose pose)
        {
            var poseNoId = new Pose()
            {
                PoseName = pose.PoseName,
                PoseOriginName = pose.PoseOriginName,
                PoseOriginStyle = pose.PoseOriginStyle
            };

            var result = await _poseRepository.AddPoseToDb(poseNoId);

            return Ok(result);
        }

        // DELETE: api/Poses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePose(int id)
        {
            Pose deletedPose = await _poseRepository.DeletePose(id);

            return Ok(deletedPose);
        }
    }
}
