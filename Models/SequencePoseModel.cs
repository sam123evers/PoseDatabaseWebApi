namespace PoseDatabaseWebApi.Models
{
    public class SequencePoseModel
    {
        public int? SequencePoseId { get; set; }
        public int SequenceId { get; set; }
        public int PoseId { get; set; }
        public int? PoseOrder { get; set; }
    }
}
