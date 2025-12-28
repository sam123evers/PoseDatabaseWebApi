namespace PoseDatabaseWebApi.Data.Dto.Session
{
    public class SessionDto
    {
        public int? SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string SessionAlternateName { get; set; } = string.Empty;
    }
}
