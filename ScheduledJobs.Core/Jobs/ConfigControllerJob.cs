using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;
using ScheduledJobs.Core.Services;

namespace ScheduledJobs.Core.Jobs
{
    public class ConfigControllerJob : ISchedulerJob
    {
        public ScheduledJob? JobModel { get; set; }
        public ConfigurationService? ConfigService { get; set; }
        protected JobEngineServiceCore? JobEngineService { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            JobEngineService = (JobEngineServiceCore)context.JobDetail.JobDataMap[nameof(JobEngineServiceCore)];
            ConfigService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(Services.ConfigurationService)];
            JobModel = (ScheduledJob)context.JobDetail.JobDataMap[nameof(ScheduledJob)];

            Console.WriteLine($"{DateTime.Now} : {nameof(ConfigControllerJob)} trigged");
            try
            {
                JobEngineService.SetConfiguration(ConfigService.GetConfiguration());
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
