using Quartz;
using ScheduledJobs.Core.Models;
using ScheduledJobs.Core.Services;

namespace ScheduledJobs.Core.Interfaces
{
    public interface ISchedulerJob : IJob
    {
        public ScheduledJobModel JobModel { get; set; }

        public ConfigurationService ConfigurationService { get; set; }

    }
}
