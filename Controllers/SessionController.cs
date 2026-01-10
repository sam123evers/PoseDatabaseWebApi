using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Service;
using System.Security.Claims;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService service)
        {
            _sessionService = service;
        }

        [HttpPost]
        [Route("CreateSession")]
        [Authorize]
        public async Task<int> CreateSessionAsync([FromBody] SessionModel createSessionInput)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sessionService.CreateSession(createSessionInput, userId);
        }

        [HttpPut]
        [Route("UpdateSession")]
        [Authorize]
        public async Task<int> UpdateSessionAsync([FromBody] SessionModel updateSessionInput)
        {
            return await _sessionService.UpdateSession(updateSessionInput);
        }

        [HttpGet]
        [Route("GetMySessions")]
        [Authorize]
        public async Task<List<SessionModel>> GetMySessionsAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sessionService.GetMySessionListWithSequences(userId!);
        }

        [HttpGet]
        [Route("GetAllSessions")]
        [Authorize]
        public async Task<List<SessionModel>> GetAllSessionsAsync()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _sessionService.GetAllSessionListWithSequences();
        }
    }
}
