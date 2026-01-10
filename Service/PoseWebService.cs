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

        public async Task<List<PoseModel>> GetPoseList()
        {
            List<PoseDto> poses = await _poseDataAccess.SelectPoseListAsync();

            return _mapper.Map<List<PoseModel>>(poses);
        }

        public async Task<List<PoseModel>> SearchPoses(string searchTerm)
        {
            List<PoseDto> poses = await _poseDataAccess.SearchPosesAsync(searchTerm);

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
}

