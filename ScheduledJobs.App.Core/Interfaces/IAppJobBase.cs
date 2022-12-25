using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.App.Core.Interfaces
{
    public interface IAppJobBase : ISchedulerJob
    {
        T CreateContract<T, M>(M model) where T : class;
        T CreateLogModel<T, M>(M model, OperationResult serviceresult) where T : class;
        T GetInternalServiceResult<T>() where T : class;
        void PopulateJobDataMap(IJobExecutionContext context);
    }
}
