namespace PoseDatabaseWebApi.Models
{
    public class PoseModel
    {
        public int? PoseId { get; set; }
        public string PoseName { get; set; }
        public string PhotoUrl { get; set; }

        public int SequencePoseId { get; set; }
    }
}
