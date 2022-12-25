using Microsoft.Extensions.Configuration;
using ScheduledJobs.Models;
using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Data.Services;

namespace ScheduledJobs.Core.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration configuration;
        private readonly IDataService dataService;
        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
            //this.dataService = dataService;
            dataService = new DummyDataService();
        }

        public string[]? JobProjects => configuration.GetSection("ProjectSettings:JobProjects").Get<string[]>();

        public List<ScheduledJob> GetConfiguration(string[]? projectList = null)
        {
            List<ScheduledJob> configuration = new();
            try
            {
                string[]? projects;
                if (projectList is not null && projectList.Any())
                {
                    projects = projectList;
                }
                else
                {
                    if (JobProjects is not null && JobProjects.Any())
                    {
                        projects = JobProjects;
                    }
                    else
                    {
                        projects = new string[] { System.Reflection.Assembly.GetEntryAssembly().GetName().Name };
                    }
                }

                configuration = MapModel(dataService.GetJobs().ToList());
            }
            catch (Exception exp)
            {

                Console.WriteLine($"Job konfigürasyonu okunurken hata oluştu. @{this.GetType().FullName}. Hata : {exp.Message}");
            }

            return configuration;
        }

        private List<ScheduledJob> MapModel(List<ScheduledJob> serviceModel)
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
