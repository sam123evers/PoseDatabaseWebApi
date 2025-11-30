namespace PoseDatabaseWebApi.Data.Dto.Identity.User
{
    public class UpdateAspNetUserDto
    {
        public string Id { get; set; } = String.Empty;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } = String.Empty;
        public string? FirstName { get; set; }
        //public string? LastName { get; set; }

        //  Normalize server-side: compute NormalizedUserName / NormalizedEmail using UserManager.NormalizeName()
        //  or ToUpperInvariant() patterns — don't accept normalized values from clients.
    }
}
