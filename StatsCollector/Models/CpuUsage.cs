using System;
using System.Collections.Generic;
using System.Text;

namespace StatsCollector.Models
{
    public class CpuUsage
    {
        public int Id { get; set; }
        public int Usage { get; set; }
        public DateTime Time { get; set; }
    }
}
