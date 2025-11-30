using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }

        //[HttpGet]
        //[Route("GetUsers")]
        //public async Task<List<UserDataModel>> GetUsersAsync()
        //{
        //    return await _poseWebService.GetUserData();
        //}

        [HttpPost]
        [Route("CreateUser")]
        public async Task<bool> CreateUserAsync([FromBody] UserDataModel createUserInput)
        {
            return await _userService.CreateNewUserAsync(createUserInput.UserName, createUserInput.Password, createUserInput.Email);
        }

        //[HttpPut]
        //[Route("UpdateUser")]
        //public async Task<int> UpdateUserAsync([FromBody] UpdateUserDataModel updateUserInput)
        //{
        //    return await _poseWebService.UpdateUser(updateUserInput);
        //}

        //[HttpDelete]
        //[Route("DeleteUser/{userDataId}")]
        //public async Task<int> DeleteUserAsync([FromRoute]int userDataId)
        //{
        //    return await _poseWebService.SetDeleteUser(userDataId);
        //}
    }
}
