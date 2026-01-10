namespace PoseDatabaseWebApi.Data.Dto.Sequence
{
    public class AddPoseToSequenceDto
    {
        public int SequenceId { get; set; }
        public int PoseId { get; set; }
        public int? PoseOrder { get; set; }
    }
}
