using Quartz.Impl;
using ScheduledJobs.Core.Models;
using ScheduledJobs.Core.Jobs;
using ScheduledJobs.Core.Extensions;

namespace ScheduledJobs.Core.Services
{
    public class JobEngineCoreService : JobEngineBaseService
    {
        protected List<ScheduledJobModel>? lastConfiguration { get; set; }
        protected List<ScheduledJobModel>? currentConfiguration { get; set; }
        protected ConfigControllerJob controllerJob { get; set; }

        public JobEngineCoreService(StdSchedulerFactory stdSchedulerFactory
                            , ConfigurationService configurationservice
                            , ConfigControllerJob controllerjob
                            ) : base(stdSchedulerFactory, configurationservice)
        {
            this.controllerJob = controllerjob;
        }

        public virtual void Run()
        {
            scheduler.Start().Wait();
            currentConfiguration = new();
            if (UserControllerJob)
            {
                AddJob(new ScheduledJobModel
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
                            PeriodAsCron = JobEngineBaseService.CronPeriod,
                            Active = true
                        }
                    }
                });
            }
        }
        public void AddJob(ScheduledJobModel job)
        {
            _ = PopulateJob(job);
        }
        public void PopulateJobs()
        {
            PopulateJobs(GetConfiguration().Where(x => x.Active).ToList().Where(x => x.JobDetail.Active).ToList()).GetAwaiter().GetResult();
        }
        public void SetConfiguration(List<ScheduledJobModel> configuration)
        {
            currentConfiguration = new();
            currentConfiguration.AddRange(configuration);
        }
        private List<ScheduledJobModel> GetConfiguration()
        {
            return currentConfiguration;
        }

        /// <summary>
        /// Joblara dair db seviyesinde bir değişiklik olup olmadığını kontrol eden metod.
        /// Eklenen, pasife çekilen, silinen veya periyodu değişen jobları bu değişikliklerin çalışma zamanında uygulanması için tespit eder.
        /// </summary>
        public void CheckChanges()
        {
            List<ScheduledJobModel> Added = new();
            List<ScheduledJobModel> Removed = new();
            List<ScheduledJobModel> Changed = new();

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

                foreach (ScheduledJobModel lastConfig in lastConfiguration)
                {
                    foreach (ScheduledJobModel newConfig in currentConfiguration)
                    {
                        if (lastConfig.JobIdentity == newConfig.JobIdentity && !lastConfig.IsEqual(newConfig))
                        {
                            Changed.Add(newConfig);
                        }
                    }
                }
            }

            lastConfiguration = currentConfiguration.ToList();
            ApplyChanges(Added, Removed, Changed);
        }

        private void ApplyChanges(List<ScheduledJobModel> Added, List<ScheduledJobModel> Removed, List<ScheduledJobModel> Changed)
        {
            if (Changed.Count > 0)
            {
                List<ScheduledJobModel> tmpChanged = new();
                foreach (var chn in Changed)
                {
                    if (!chn.Active || !chn.JobDetail.Active)
                    {
                        tmpChanged.Add(chn);
                    }
                }

                if (tmpChanged.Any())
                {
                    RemoveJobs(tmpChanged).GetAwaiter().GetResult();
                }

                tmpChanged = new List<ScheduledJobModel>();
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
                RemoveJobs(Removed.ToList()).GetAwaiter().GetResult();
                Removed.Clear();
            }

            if (Added.Count > 0)
            {
                PopulateJobs(Added.Where(x => x.Active).ToList().Where(x => x.JobDetail.Active).ToList()).GetAwaiter().GetResult();
                Added.Clear();
            }
        }
    }
}
