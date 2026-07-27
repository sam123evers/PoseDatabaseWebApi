using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PoseDatabaseWebApi.Data.Identity
{
    public class AppUsersDbContextFactory : IDesignTimeDbContextFactory<AppUsersDbContext>
    {
        public AppUsersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppUsersDbContext>();

            // Get connection string from configuration
            string connectionString = ConfigurationHelper.GetConnectionString("Identity");

            optionsBuilder.UseNpgsql(connectionString, options => 
            { 
                options.SetPostgresVersion(18, 0); 
            });

            return new AppUsersDbContext(optionsBuilder.Options);
        }
    }
}
