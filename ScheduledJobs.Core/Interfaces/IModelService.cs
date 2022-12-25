using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Interfaces
{
    public interface IModelService
    {
        List<ScheduledJob> Map(List<ScheduledJob> serviceModel);
    }
}
