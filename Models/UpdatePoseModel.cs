namespace PoseDatabaseWebApi.Models
{
    public class UpdatePoseModel
    {
        public int PoseId { get; set; }
        public string? PoseName { get; set; }
        public string? PhotoUrl { get; set; }
        public int[]? PoseVariations { get; set; }
    }
}
