namespace PoseDatabaseWebApi.Data.Dto.Identity.RoleClaims
{
    public class AspNetRoleClaimDto
    {
        public int Id { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}
