using System;
using System.Collections.Generic;
using System.Text;

namespace StatsPublisher.Models
{
    public class CpuUsage
    {
        public int Id { get; set; }
        public int Usage { get; set; }
        public DateTime Time { get; set; }
    }
}
