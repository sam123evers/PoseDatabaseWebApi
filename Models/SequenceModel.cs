namespace PoseDatabaseWebApi.Models
{
    public class SequenceModel
    {
        public int? SequenceId { get; set; }
        public string SequenceName { get; set; } = String.Empty;
        public string? SequenceAlternateName { get; set; } = String.Empty;

        public List<PoseModel>? Poses { get; set; } = new();
    }
}
