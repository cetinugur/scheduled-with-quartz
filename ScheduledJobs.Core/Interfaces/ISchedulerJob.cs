using Quartz;
using ScheduledJobs.Core.Services;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Interfaces
{
    public interface ISchedulerJob : IJob
    {
        public ScheduledJob JobModel { get; set; }

        public ConfigurationService ConfigurationService { get; set; }

    }
}
