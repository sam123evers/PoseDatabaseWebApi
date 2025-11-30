namespace PoseDatabaseWebApi.Data.Dto.Identity.User
{
    public class AspNetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Normalized_UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public  string Normalized_Email { get; set; } = String.Empty;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = String.Empty;
        public string SecurityStamp { get; set; } = String.Empty;
        public string ConcurrencyStamp { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public bool IsDeleted { get; set; }
    }
}
