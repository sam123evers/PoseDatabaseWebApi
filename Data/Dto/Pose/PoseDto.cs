namespace PoseDatabaseWebApi.Data.Dto.Pose
{
    public class PoseDto
    {
        public int?  PoseId { get; set; }
        public string PoseName { get; set; }
        public string PhotoUrl { get; set; }
        public int[]? PoseVariations { get; set; }

    }
}
