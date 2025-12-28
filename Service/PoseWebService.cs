using AutoMapper;
using PoseDatabaseWebApi.Data.App;

//using PoseDatabaseWebApi.Data.Dto.Identity;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Models;
//using System;

namespace PoseDatabaseWebApi.Service;

    public class PoseWebService : IPoseWebService
    {
        private readonly IPoseDataAccess _poseDataAccess;
        private readonly IMapper _mapper;
        public PoseWebService(IPoseDataAccess poseWebData, IMapper mapper) 
        {
            _poseDataAccess = poseWebData;
            _mapper = mapper;
        }

    //#region User Methods
    //public async Task<List<UserDataModel>> GetUserData()
    //{
    //    List<UserDto> users = await _poseWebData.GetUsersAsync();
    //    // one-line? nah...
    //    return _mapper.Map<List<UserDataModel>>(users);
    //}

    //public async Task<int> CreateUser(UserDataModel userCreateObj)
    //{
    //    return await _poseWebData.CreateUserAsync(_mapper.Map<UserDto>(userCreateObj));
    //}

    //public async Task<int> UpdateUser(UpdateUserDataModel userUpdateObj)
    //{
    //    return await _poseWebData.UpdateUserAsync(_mapper.Map<UpdateUserDto>(userUpdateObj));
    //}

    //public async Task<int> SetDeleteUser(int userDataId)
    //{
    //    return await _poseWebData.SetDeleteUserAsync(userDataId);
    //}

    //#endregion

    #region Pose Methods

    public async Task<List<PoseModel>> GetPoseList()
    {
        List<PoseDto> poses = await _poseDataAccess.SelectPoseListAsync();

        return _mapper.Map<List<PoseModel>>(poses);
    }

    public async Task<int> CreatePose(PoseModel poseCreateObj)
    {
        return await _poseDataAccess.InsertPoseAsync(_mapper.Map<PoseDto>(poseCreateObj));
    }

    public async Task<int> UpdatePose(UpdatePoseModel poseUpdateObj)
    {
        return await _poseDataAccess.UpdatePoseAsync(_mapper.Map<UpdatePoseDto>(poseUpdateObj));
    }

    public async Task<int> SetDeletePose(int poseId)
    {
        return await _poseDataAccess.SetDeletePoseAsync(poseId);
    }

    #endregion
}

