using ScheduledJobs.Models;

namespace ScheduledJobs.Data.Interfaces
{
    public interface IDataService
    {
        IEnumerable<ScheduledJob> GetJobs();
    }
}
