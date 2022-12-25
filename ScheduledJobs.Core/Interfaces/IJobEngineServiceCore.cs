using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Interfaces
{
    public interface IJobEngineServiceCore
    {
        public void Run();
        void AddJob(ScheduledJob job);
        void PopulateJobs();
        void SetConfiguration(List<ScheduledJob> configuration);
        void CheckChanges();
    }
}
