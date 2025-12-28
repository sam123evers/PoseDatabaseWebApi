using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoseDatabaseWebApi.Models.Identity;

namespace PoseDatabaseWebApi.Data.Identity
{
    public class AppUsersDbContext : IdentityDbContext<AppUserModel>
    {
        public AppUsersDbContext(DbContextOptions<AppUsersDbContext> options) : base(options) { }
    }
}
