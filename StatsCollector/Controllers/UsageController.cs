using Microsoft.AspNetCore.Mvc;
using StatsCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatsCollector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsageController : Controller
    {
        private MonitorDbContext _dbContext;
        public UsageController(MonitorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("publish")]
        public IActionResult ReportUsage([FromBody] UsageStats stats)
        {
            _dbContext.CpuUsages.Add(new CpuUsage()
            {
                Time = stats.Time,
                Usage = stats.CpuUsage
            });
            _dbContext.MemoryUsages.Add(new MemoryUsage()
            {
                Time = stats.Time,
                Usage = stats.MemoryUsage
            });
            _dbContext.SaveChanges();
            return Ok();

        }
    }
}
