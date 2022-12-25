using ScheduledJobs.Core.Services;

namespace ScheduledJobs.App.Core.Engines
{
    public class ScheduledJobsEngine
    {
        private readonly JobEngineCoreService jobEngine;
        private readonly ConfigurationService configurationService;
        public ScheduledJobsEngine(JobEngineCoreService jobEngine, ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
            this.jobEngine = jobEngine;
        }

        public void Run()
        {
            jobEngine.Run();
            try
            {
                jobEngine.SetConfiguration(configurationService.GetConfiguration());
                jobEngine.PopulateJobs();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
