using Microsoft.AspNetCore.Identity;

namespace PoseDatabaseWebApi.Models.Identity
{
    public class AppUserModel : IdentityUser 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
