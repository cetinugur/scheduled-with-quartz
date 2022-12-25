using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.App.Core.Interfaces
{
    public interface IAppJobBase : ISchedulerJob
    {
        void PopulateJobDataMap(IJobExecutionContext context);
        T CreateContract<T, M>(M model) where T : class;
        T GetDataFromSource<T>() where T : class;
    }
}
