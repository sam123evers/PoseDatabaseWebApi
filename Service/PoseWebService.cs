using PoseDatabaseWebApi.Data;
using PoseDatabaseWebApi.Data.Dto;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service;

    public class PoseWebService : IPoseWebService
    {
        private readonly IPoseWebData _poseWebData;
        public PoseWebService(IPoseWebData poseWebData) 
        { 
            _poseWebData = poseWebData;
        }

    public async Task<List<UserDataModel>> GetUserData()
    {
        var users = await _poseWebData.GetUsersAsync();

        List<UserDataModel> userModelList = new();

        foreach (var user in users)
        {

            userModelList.Add(
                 new UserDataModel()
                 {
                     UserDataId = user.UserDataId ?? -1,
                     FirstName = user.FirstName,
                     LastName = user.LastName,
                     Email = user.Email
                 }
            );
        }

        return userModelList;
    }

    public async Task<int> CreateUser(UserDataModel userCreateObj)
    {
        UserDto tansformedModel = new()
        {
            FirstName = userCreateObj.FirstName,
            LastName = userCreateObj.LastName,
            Email = userCreateObj.Email,
            UserName = userCreateObj.UserName,
            IsDeleted = false
        };
        return await _poseWebData.CreateUserAsync(tansformedModel);
    }

}

