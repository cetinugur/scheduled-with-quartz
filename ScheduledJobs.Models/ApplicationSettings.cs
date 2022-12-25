namespace ScheduledJobs.Models
{
    public class ApplicationSettings
    {
        public string[]? JobProjects { get; set; }
        public ConfigControllerJobSettings ConfigControllerJobSettings { get; set; }
    }

    public class ConfigControllerJobSettings
    {
        public bool UseConfigControllerJob { get; set; }
        public string PeriodAsCron { get; set; }
    }
}
