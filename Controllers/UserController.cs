using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Models;
using System.Xml.Serialization;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPoseWebService _poseWebService;
        public UserController(IPoseWebService service)
        {
            _poseWebService = service;
        }

        [Route("GetUsers")]
        public async Task<List<UserDataModel>> GetUsersAsync()
        {
            return await _poseWebService.GetUserData();
        }

        [Route("CreateUser")]
        public async Task<int> CreateUserAsync([FromBody]UserDataModel createUserInput)
        {
            return await _poseWebService.CreateUser(createUserInput);
        }

    }
}
