using ScheduledJobs.Core.Services;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Interfaces
{
    public interface IJobService
    {
        void Start();
        Task PopulateJobs(List<ScheduledJob> jobs, JobEngineServiceCore refererService);
        Task PopulateJob(ScheduledJob model, JobEngineServiceCore refererService);
        Task RemoveJobs(List<ScheduledJob> models);
        string GetTriggerIdentityKey(ScheduledJob model);
        string GetJobIdentityKey(ScheduledJob model);
        Type? GetJobTypeFromModel(ScheduledJob model);
    }
}
