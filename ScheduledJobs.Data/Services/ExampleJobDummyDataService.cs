using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.Data.Services
{
    public class ExampleJobDummyDataService : IDataService
    {
        IEnumerable<ScheduledJob> IDataService.GetJobs()
        {
            List<ScheduledJob> result = new()
            {
                new ScheduledJob
                {
                    Id = 1,
                    Active = true,
                    Project = "ScheduledJobs.App.ImpExample",
                    Description = "ScheduledJobs.App.ImpExample.ExampleJob sınıfını uygulatacak.",
                    JobName = "ScheduledJobs.App.ImpExample.Jobs.ExampleJob",
                    JobDetails = new List<ScheduledJobDetail>
                {
                   new ScheduledJobDetail
                   {
                       Id = 1,
                       Active = true,
                       JobId = 1,
                       LastExecutionTime = null,
                       Name = "ScheduledJobs.App.ImpExample.ExampleJob 1",
                       PeriodAsCron = "0/2 * * * * ?",
                   },
                   new ScheduledJobDetail
                   {
                       Id = 2,
                       Active = true,
                       JobId = 1,
                       LastExecutionTime = null,
                       Name = "ScheduledJobs.App.ImpExample.ExampleJob 2",
                       PeriodAsCron = "0/04 * * * * ?",
                   }
                }
                }
            };

            return result;
        }
    }
}
