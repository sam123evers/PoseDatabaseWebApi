using Microsoft.EntityFrameworkCore;

namespace PoseDatabaseWebApi.Models
{
    public class PoseContext : DbContext
    {
        public PoseContext(DbContextOptions<PoseContext> options) : base(options) { }

        public DbSet<Pose> Poses { get; set; }
    }
}