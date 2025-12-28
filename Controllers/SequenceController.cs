using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Models.Identity;
using PoseDatabaseWebApi.Service;
using System.Security.Claims;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController : ControllerBase
    {
        private readonly ISequenceService _sequenceService;
        //private readonly UserManager<AppUserModel> _userManager;
        public SequenceController(ISequenceService service
            //, UserManager<AppUserModel> userManager
            )
        {
            _sequenceService = service;
            //_userManager = userManager;
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

        [HttpPost]
        [Route("CreateSequence")]
        [Authorize]
        public async Task<int> CreateSequenceAsync([FromBody] SequenceModel seqObj)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sequenceService.CreateSequence(seqObj, userId);
        }
    }
}
