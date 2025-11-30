namespace PoseDatabaseWebApi.Data.Dto.Identity.UserTokens
{
    public class AspNetUserTokenDto
    {
        public string Id { get; set; } = string.Empty;
        public string LoginProvider { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
