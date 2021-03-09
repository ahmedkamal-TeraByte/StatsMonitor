using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatsCollector.Models
{
    public class UsageStats
    {
        public int CpuUsage { get; set; }
        public int MemoryUsage { get; set; }
        public DateTime Time { get; set; }

    }
}
