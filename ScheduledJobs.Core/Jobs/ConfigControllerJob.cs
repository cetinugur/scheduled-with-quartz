using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Core.Models;
using ScheduledJobs.Core.Services;

namespace ScheduledJobs.Core.Jobs
{
    public class ConfigControllerJob : ISchedulerJob
    {
        public ScheduledJobModel? JobModel { get; set; }
        public ConfigurationService? ConfigurationService { get; set; }

        protected JobEngineCoreService? JobEngineService { get; set; }


        public Task Execute(IJobExecutionContext context)
        {
            JobEngineService = (JobEngineCoreService)context.JobDetail.JobDataMap[nameof(JobEngineBaseService)];
            ConfigurationService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(Services.ConfigurationService)];
            JobModel = (ScheduledJobModel)context.JobDetail.JobDataMap[nameof(ScheduledJobModel)];

            Console.WriteLine($"{DateTime.Now} : JobController trigged");
            try
            {
                JobEngineService.SetConfiguration(ConfigurationService.GetConfiguration());
                JobEngineService.CheckChanges();
            }
            catch (Exception exp)
            {
                // TODO bak
            }

            Console.WriteLine($"{DateTime.Now} : JobController succeeded");
            return Task.CompletedTask;
        }
    }
}
