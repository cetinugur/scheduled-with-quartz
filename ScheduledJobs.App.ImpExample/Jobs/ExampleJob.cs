using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Core.Services;
using ScheduledJobs.Models;

namespace ScheduledJobs.App.ImpExample.Jobs
{
    [DisallowConcurrentExecution] // Zamanlanmış görevlenmeden bu tipte yeni bir instance oluşturulup işletilmesi izin vermez, kuyruğa alır.
    public class ExampleJob : ISchedulerJob
    {
        public ScheduledJob? JobModel { get; set; }
        public ConfigurationService? ConfigurationService { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobModel = (ScheduledJob)context.JobDetail.JobDataMap[nameof(ScheduledJob)];
                ConfigurationService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(ConfigurationService)];

                string message = $"{DateTime.Now.ToLocalTime()} : {JobModel.Description} job {JobModel.JobDetail.Name} örneği için çalıştı. Çalışma periyodu {JobModel.JobDetail.PeriodAsCron}";
                Console.WriteLine(message);

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
