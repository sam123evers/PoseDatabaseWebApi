using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Service;
using System.Security.Claims;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController : ControllerBase
    {
        private readonly ISequenceService _sequenceService;
        public SequenceController(ISequenceService service)
        {
            _sequenceService = service;
        }

        [HttpGet]
        [Route("GetSequences")]
        public async Task<List<SequenceModel>> GetSequencesAsync()
        {
            return await _sequenceService.GetSequenceList();
        }

        [HttpGet]
        [Route("GetSequenceById/{id}")]
        [Authorize]
        public async Task<SequenceModel> GetSequenceByIdAsync([FromRoute] int id)
        {
            return await _sequenceService.GetSequenceByIdAsync(id);
        }

        [HttpGet]
        [Route("SequencesAndPoses/{sessionId}")]
        public async Task<List<SequenceModel>> GetSeqAndPoseListBySeshId([FromRoute] int sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sequenceService.GetSequencesAndPosesBySeshIdAsync(sessionId);
        }

        [HttpPost]
        [Route("CreateSequence")]

        public async Task<int> CreateSequenceAsync([FromBody] SequenceCreateModel newSeqObj)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sequenceService.CreateSequence(newSeqObj);
            //    return await _sequenceService.CreateSequence(newSeqObj, userId!);
        }

        [HttpPut]
        [Route("UpdateSequence")]
        [Authorize]
        public async Task<int> UpdateSequenceAsync([FromBody] SequenceModel updateSeqInput)
        {
            return await _sequenceService.UpdateSequence(updateSeqInput);
        }

        [HttpPost]
        [Route("AddPoseToSequence")]
        //[Authorize]
        public async Task<bool> AddPoseToSequenceAsync([FromBody] SequencePoseModel seqPoseObj)
        {
            return await _sequenceService.AddPoseToSequence(seqPoseObj); ;
        }

        [HttpDelete]
        [Route("RemovePoseFromSequence/{seqPoseId}")]
        //[Authorize]
        public async Task<bool> RemovePoseFromSequenceAsync([FromRoute] int seqPoseId)
        {
            return await _sequenceService.RemovePoseFromSequence(seqPoseId);
        }
    }
}
