using Microsoft.EntityFrameworkCore;

namespace StatsCollector.Models
{
    public class MonitorDbContext : DbContext
    {
        public MonitorDbContext()
        {

        }
        public MonitorDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CpuUsage> CpuUsages { get; set; }
        public DbSet<MemoryUsage> MemoryUsages { get; set; }
    }
}
