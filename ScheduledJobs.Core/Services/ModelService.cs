using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Services
{
    public class ModelService : IModelService
    {
        public List<ScheduledJob> Map(List<ScheduledJob> serviceModel)
        {
            List<ScheduledJob> result = new();

            var details = serviceModel.SelectMany(x => x.JobDetails).ToList();
            foreach (var srvModelDetailItem in details)
            {
                var jobModelParent = serviceModel.FirstOrDefault(x => x.Id == srvModelDetailItem.JobId);

                ScheduledJob jobModel = new()
                {
                    Id = srvModelDetailItem.JobId,
                    Active = jobModelParent.Active,
                    JobName = jobModelParent.JobName,
                    Project = jobModelParent.Project,
                    Description = jobModelParent.Description,
                    JobDetails = new List<ScheduledJobDetail>
                    {
                        new ()
                        {
                            Id = srvModelDetailItem.Id,
                            Active = srvModelDetailItem.Active,
                            PeriodAsCron = srvModelDetailItem.PeriodAsCron,
                            JobId = srvModelDetailItem.JobId,
                            Name = srvModelDetailItem.Name
                        }
                    }
                };

                result.Add(jobModel);
            }

            return result;
        }
    }
}
