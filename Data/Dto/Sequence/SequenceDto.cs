using PoseDatabaseWebApi.Data.Dto.Pose;

namespace PoseDatabaseWebApi.Data.Dto.Sequence
{
    public class SequenceDto
    {
        public int? SequenceId { get; set; }
        public string SequenceName { get; set; } = String.Empty;
        //public string? SequenceAlternateName { get; set; }
        public List<PoseDto> Poses { get; set; } = new();
    }
}
