using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Interfaces
{
    public interface IConfigurationService
    {
        public List<ScheduledJob> GetConfiguration(string[]? projectList = null);
    }
}
