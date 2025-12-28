using AutoMapper;
using PoseDatabaseWebApi.Data.App;
//using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Data.Dto.Sequence;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public class SequenceService : ISequenceService
    {
        private readonly ISequenceDataAccess _sequenceDataAccess;
        private readonly IMapper _mapper;
        public SequenceService(ISequenceDataAccess sequenceDataAccess, IMapper mapper)
        {
            _sequenceDataAccess = sequenceDataAccess;
            _mapper = mapper;
        }

        public async Task<List<SequenceModel>> GetSequenceList()
        {
            List<SequenceDto> sequences = await _sequenceDataAccess.SelectSequenceListAsync();

            return _mapper.Map<List<SequenceModel>>(sequences);
        }

        public async Task<SequenceModel> GetSequenceByIdAsync(int sequenceId)
        {
            SequenceDto sequence = await _sequenceDataAccess.GetSequenceByIdAsync(sequenceId);

            return _mapper.Map<SequenceModel>(sequence);
        }

        public async Task<int> CreateSequence(SequenceModel seqCreateObj, string loggedInUserId)
        {
            return await _sequenceDataAccess.InsertSequenceAsync(_mapper.Map<SequenceDto>(seqCreateObj), loggedInUserId);
        }
    }
}
