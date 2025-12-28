using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;
using PoseDatabaseWebApi.Service;

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
        public async Task<int> CreateSessionAsync([FromBody] SessionModel createSessionInput)
        {
            return await _sessionService.CreateSession(createSessionInput);
        }
    }
}
