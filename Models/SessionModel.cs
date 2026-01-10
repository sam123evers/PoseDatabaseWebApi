namespace PoseDatabaseWebApi.Models
{
    public class SessionModel
    {
        public int? SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string SessionAlternateName { get; set; } = string.Empty;

        public List<SequenceModel> Sequences { get; set; } = new();
    }
}
