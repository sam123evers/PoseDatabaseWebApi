namespace PoseDatabaseWebApi.Data.Dto.Identity.UserClaims
{
    public class AspNetUserClaimDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string ClaimType { get; set; } = string.Empty;

        public string ClaimValue { get; set; } = string.Empty;
    }
}
