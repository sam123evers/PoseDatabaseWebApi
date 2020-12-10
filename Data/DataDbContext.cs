using Microsoft.EntityFrameworkCore;
using PoseDatabaseWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseDatabaseWebApi.Data
{
    public class PoseDataContext : DbContext
    {
        public PoseDataContext(DbContextOptions<PoseDataContext> options) : base(options)
        { }

        public DbSet<Pose> Poses { get; set; }
    }
}
