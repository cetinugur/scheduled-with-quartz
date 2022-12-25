using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;
using ScheduledJobs.Core.Services;

namespace ScheduledJobs.Core.Jobs
{
    public class ConfigControllerJob : ISchedulerJob
    {
        public ScheduledJob? JobModel { get; set; }
        public ConfigurationService? ConfigurationService { get; set; }
        protected JobEngineCoreService? JobEngineService { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            JobEngineService = (JobEngineCoreService)context.JobDetail.JobDataMap[nameof(JobEngineBaseService)];
            ConfigurationService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(Services.ConfigurationService)];
            JobModel = (ScheduledJob)context.JobDetail.JobDataMap[nameof(ScheduledJob)];

            Console.WriteLine($"{DateTime.Now} : {nameof(ConfigControllerJob)} trigged");
            try
            {
                JobEngineService.SetConfiguration(ConfigurationService.GetConfiguration());
                JobEngineService.CheckChanges();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }

            Console.WriteLine($"{DateTime.Now} : {nameof(ConfigControllerJob)} succeeded");
            return Task.CompletedTask;
        }
    }
}
