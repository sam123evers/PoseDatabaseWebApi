using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Service;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoseController : ControllerBase
    {
        private readonly IPoseWebService _poseWebService;

        public PoseController(IPoseWebService service)
        {
            _poseWebService = service;
        }

        [HttpGet]
        [Route("GetPoses")]
        [Authorize]
        public async Task<List<PoseModel>> GetPosesAsync()
        {
            return await _poseWebService.GetPoseList();
        }

        [HttpPost]
        [Route("SearchPoses")]
        [Authorize]
        public async Task<List<PoseModel>> SearchPosesAsync([FromBody] string searchTerm)
        {
            return await _poseWebService.SearchPoses(searchTerm);
        }

        [HttpPost]
        [Route("CreatePose")]
        public async Task<int> CreatePoseAsync([FromBody] PoseModel createPoseInput)
        {
            return await _poseWebService.CreatePose(createPoseInput);
        }

        [HttpPut]
        [Route("UpdatePose")]
        [Authorize]
        public async Task<int> UpdatePoseAsync([FromBody] UpdatePoseModel updatePoseInput)
        {
            return await _poseWebService.UpdatePose(updatePoseInput);
        }

        [HttpDelete]
        [Route("DeletePose/{poseId}")]
        [Authorize]
        public async Task<int> DeletePoseAsync([FromRoute] int poseId)
        {
            return await _poseWebService.SetDeletePose(poseId);
        }
    }
}
