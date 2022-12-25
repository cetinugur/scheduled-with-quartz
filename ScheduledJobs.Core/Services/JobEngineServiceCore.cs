using ScheduledJobs.Models;
using ScheduledJobs.Core.Jobs;
using ScheduledJobs.Core.Extensions;
using ScheduledJobs.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ScheduledJobs.Core.Services
{
    public class JobEngineServiceCore : IJobEngineServiceCore
    {
        protected List<ScheduledJob>? lastConfiguration { get; set; }
        protected List<ScheduledJob>? currentConfiguration { get; set; }
        protected ConfigControllerJob controllerJob { get; set; }
        private readonly IJobService jobService;
        private readonly IConfiguration configuration;
        public JobEngineServiceCore(ConfigControllerJob controllerjob,
            IJobService jobService,
            IConfiguration configuration)

        {
            this.controllerJob = controllerjob;
            this.jobService = jobService;
            this.configuration = configuration;

        }

        public virtual void Run()
        {
            jobService.Start();
            currentConfiguration = new();

            var configControllerJobSettings = configuration?.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>().ConfigControllerJobSettings;

            if (configControllerJobSettings.UseConfigControllerJob)
            {
                AddJob(new ScheduledJob
                {
                    Id = -1,
                    Active = true,
                    JobName = nameof(ConfigControllerJob),
                    Project = typeof(ConfigControllerJob).Namespace,
                    Description = "Değişiklikleri düzenli kontrol eden zorunlu job.",
                    JobDetails = new()
                    {
                        new ScheduledJobDetail
                        {
                            Id = -1,
                            JobId = -1,
                            PeriodAsCron = configControllerJobSettings .PeriodAsCron,
                            Active = true
                        }
                    }
                });
            }
        }
        public void AddJob(ScheduledJob job)
        {
            _ = jobService.PopulateJob(job, this);
        }
        public void PopulateJobs()
        {
            jobService.PopulateJobs(currentConfiguration?.Where(x => x.Active).ToList().Where(x => x.JobDetail.Active).ToList(), this).GetAwaiter().GetResult();
        }
        public void SetConfiguration(List<ScheduledJob> configuration)
        {
            currentConfiguration = new();
            currentConfiguration.AddRange(configuration);
        }
        public void CheckChanges()
        {
            List<ScheduledJob> Added = new();
            List<ScheduledJob> Removed = new();
            List<ScheduledJob> Changed = new();

            if (lastConfiguration != null)
            {
                var tmpAdded = currentConfiguration.Where(x => !lastConfiguration.Select(y => y.Id).Contains(x.Id)).ToList();
                foreach (var item in tmpAdded)
                {
                    if (item.Active && item.JobDetail.Active)
                    {
                        Added.Add(item);
                    }
                }

                Removed = lastConfiguration.Where(x => !currentConfiguration.Select(y => y.Id).Contains(x.Id)).ToList();

                Changed.AddRange(from ScheduledJob lastConfig in lastConfiguration
                                 from ScheduledJob newConfig in currentConfiguration
                                 where lastConfig.JobIdentity == newConfig.JobIdentity && !lastConfig.IsEqual(newConfig)
                                 select newConfig);
            }

            lastConfiguration = currentConfiguration?.ToList();
            ApplyChanges(Added, Removed, Changed);
        }
        private void ApplyChanges(List<ScheduledJob> Added, List<ScheduledJob> Removed, List<ScheduledJob> Changed)
        {
            if (Changed.Count > 0)
            {
                List<ScheduledJob> tmpChanged = new();

                tmpChanged.AddRange(from chn in Changed
                                    where !chn.Active || !chn.JobDetail.Active
                                    select chn);
                if (tmpChanged.Any())
                {
                    jobService.RemoveJobs(tmpChanged).GetAwaiter().GetResult();
                }

                tmpChanged = new List<ScheduledJob>();

                foreach (var chn in Changed)
                {
                    if (chn.Active && chn.JobDetail.Active)
                    {
                        tmpChanged.Add(chn);
                    }
                }

                Changed.Clear();

                Removed.AddRange(tmpChanged);
                Added.AddRange(tmpChanged);
            }

            if (Removed.Count > 0)
            {
                jobService.RemoveJobs(Removed.ToList()).GetAwaiter().GetResult();
                Removed.Clear();
            }

            if (Added.Count > 0)
            {
                jobService.PopulateJobs(Added.Where(x => x.Active).ToList().Where(x => x.JobDetail.Active).ToList(), this).GetAwaiter().GetResult();
                Added.Clear();
            }
        }
    }
}
