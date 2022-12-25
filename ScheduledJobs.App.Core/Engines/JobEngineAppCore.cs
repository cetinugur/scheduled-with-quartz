using ScheduledJobs.Core.Interfaces;

namespace ScheduledJobs.App.Core.Engines
{
    public class JobEngineAppCore
    {
        private readonly IJobEngineServiceCore jobEngine;
        private readonly IConfigurationService configurationService;
        public JobEngineAppCore(IJobEngineServiceCore jobEngine, IConfigurationService configurationService)
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
