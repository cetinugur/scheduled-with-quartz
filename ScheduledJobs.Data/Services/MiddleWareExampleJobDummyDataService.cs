using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.Data.Services
{
    public class MiddleWareExampleJobDummyDataService : IDataService
    {
        IEnumerable<ScheduledJob> IDataService.GetJobs()
        {
            List<ScheduledJob> result = new()
            {
                new ScheduledJob
                {
                    Id = 1,
                    Active = true,
                    Project = "ScheduledJobs.App.MWImpExample",
                    Description = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob sınıfını uygulatacak.",
                    JobName = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob",
                    JobDetails = new List<ScheduledJobDetail>
                {
                   new ScheduledJobDetail
                   {
                       Id = 1,
                       Active = true,
                       JobId = 1,
                       LastExecutionTime = null,
                       Name = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob 1",
                       PeriodAsCron = "0/2 * * * * ?",
                   }
                }
                },
                    new ScheduledJob
                {
                    Id = 2,
                    Active = true,
                    Project = "ScheduledJobs.App.MWImpExample",
                    Description = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob2 sınıfını uygulatacak.",
                    JobName = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob2",
                    JobDetails = new List<ScheduledJobDetail>
                {
                   new ScheduledJobDetail
                   {
                       Id = 2,
                       Active = true,
                       JobId = 2,
                       LastExecutionTime = null,
                       Name = "ScheduledJobs.App.MWImpExample.Jobs.MiddleWareExampleJob2 1",
                       PeriodAsCron = "0/2 * * * * ?",
                   }
                }
                }
            };

            return result;
        }
    }
}
