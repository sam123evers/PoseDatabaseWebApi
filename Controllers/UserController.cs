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

        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<UserDataModel>> GetUsersAsync()
        {
            return await _poseWebService.GetUserData();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<int> CreateUserAsync([FromBody] UserDataModel createUserInput)
        {
            return await _poseWebService.CreateUser(createUserInput);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<int> UpdateUserAsync([FromBody] UpdateUserDataModel updateUserInput)
        {
            return await _poseWebService.UpdateUser(updateUserInput);
        }

        [HttpDelete]
        [Route("DeleteUser/{userDataId}")]
        public async Task<int> DeleteUserAsync([FromRoute]int userDataId)
        {
            return await _poseWebService.SetDeleteUser(userDataId);
        }

    }
}
