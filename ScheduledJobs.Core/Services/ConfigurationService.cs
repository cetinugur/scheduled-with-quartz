using Microsoft.Extensions.Configuration;
using ScheduledJobs.Core.Models;

namespace ScheduledJobs.Core.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration configuration;
        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string[]? JobProjects => configuration.GetSection("ProjectSettings:JobProjects").Get<string[]>();
        public string? ServiceUrl { get { return configuration.GetSection("ApiSettings:InternalAPIBaseUrl").Value; } }
        public int SendingDataPartition { get { return Convert.ToInt32(configuration.GetSection("ProjectSettings:SendingDataPartition")?.Value); } }

        public List<ScheduledJobModel> GetConfiguration(string[]? projectList = null)
        {
            List<ScheduledJobModel> configuration = new();
            try
            {
                string[]? projects;
                if (projectList.Any())
                {
                    projects = projectList;
                }
                else
                {
                    if (JobProjects.Any())
                    {
                        projects = JobProjects;
                    }
                    else
                    {
                        projects = new string[] { System.Reflection.Assembly.GetEntryAssembly().GetName().Name };
                    }
                }
            }
            catch (Exception exp)
            {

                Console.WriteLine($"Job konfigürasyonu okunurken hata oluştu. @{this.GetType().FullName}. Hata : {exp.Message}");
            }

            return configuration;
        }

        private List<ScheduledJobModel> MapModel(List<ScheduledJobModel> serviceModel)
        {
            List<ScheduledJobModel> result = new();

            var details = serviceModel.SelectMany(x => x.JobDetails).ToList();
            foreach (var srvModelDetailItem in details)
            {
                var jobModelParent = serviceModel.FirstOrDefault(x => x.Id == srvModelDetailItem.JobId);

                ScheduledJobModel jobModel = new()
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
