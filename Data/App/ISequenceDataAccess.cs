using PoseDatabaseWebApi.Data.Dto.Sequence;

namespace PoseDatabaseWebApi.Data.App
{
    public interface ISequenceDataAccess
    {
        Task<List<SequenceDto>> SelectSequenceListAsync();

        Task<SequenceDto> GetSequenceByIdAsync(int seqId);

        Task<int> InsertSequenceAsync(SequenceDto sequenceCreateObj, string loggedInUserId);
    }
}
