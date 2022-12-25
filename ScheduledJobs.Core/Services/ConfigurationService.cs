using Microsoft.Extensions.Configuration;
using ScheduledJobs.Models;
using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Core.Interfaces;

namespace ScheduledJobs.Core.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration configuration;
        private readonly IDataService dataService;
        private readonly IModelService modelService;
        public ConfigurationService(IConfiguration configuration, IDataService dataService, IModelService modelService)
        {
            this.configuration = configuration;
            this.dataService = dataService;
            this.modelService = modelService;
        }

        public List<ScheduledJob> GetConfiguration(string[]? projectList = null)
        {
            List<ScheduledJob> result = new();
            try
            {
                string[]? projects;

                if (projectList is not null && projectList.Any())
                {
                    projects = projectList;
                }
                else
                {
                    string[] JobProjects = configuration?.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>().JobProjects;

                    if (JobProjects is not null && JobProjects.Any())
                    {
                        projects = JobProjects;
                    }
                    else
                    {
                        projects = new string[] { System.Reflection.Assembly.GetEntryAssembly().GetName().Name };
                    }
                }

                result = modelService.Map(dataService.GetJobs().ToList());
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Job konfigürasyonu okunurken hata oluştu. @{this.GetType().FullName}. Hata : {exp.Message}");
            }

            return result;
        }
    }
}
