using PoseDatabaseWebApi.Data.Dto.Sequence;
using PoseDatabaseWebApi.Models;

namespace PoseDatabaseWebApi.Service
{
    public interface ISequenceService
    {
        Task<List<SequenceModel>> GetSequenceList();

        Task<SequenceModel> GetSequenceByIdAsync(int sequenceId);

        Task<int> CreateSequence(SequenceModel seqCreateObj, string loggedInUserId);

        Task<int> UpdateSequence(SequenceModel seqCreateObj);

        Task<bool> AddPoseToSequence(SequencePoseModel seqPoseObj);

        Task<bool> RemovePoseFromSequence(int seqPoseId);
    }
}
