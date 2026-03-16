using PoseDatabaseWebApi.Data.Dto.Sequence;

namespace PoseDatabaseWebApi.Data.App
{
    public interface ISequenceDataAccess
    {
        Task<List<SequenceDto>> SelectSequenceListAsync();

        Task<SequenceDto> GetSequenceByIdAsync(int seqId);

        Task<List<SequenceDto>> GetSequencesAndPosesBySessionIdAsync(int seshId);

        //Task<int> InsertSequenceAsync(SequenceDto sequenceCreateObj, string loggedInUserId);

        Task<int> InsertSequenceAsync(SequenceCreateDto sequenceCreateObj);

        Task<int> UpdateSequenceAsync(SequenceDto seqDto);

        Task<bool> AddPoseToSequenceAsync(SequencePoseDto seqPoseDto);

        Task<bool> RemovePoseFromSequenceAsync(int seqPoseId);
    }
}
