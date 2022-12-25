using Quartz;
using Quartz.Impl;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Services
{
    public class JobService : IJobService
    {
        private readonly IConfigurationService configurationService;
        private readonly IScheduler scheduler;

        public JobService(IConfigurationService configurationService, StdSchedulerFactory stdSchedulerFactory)
        {
            this.configurationService = configurationService;
            this.scheduler = stdSchedulerFactory.GetScheduler().Result;
        }

        public void Start()
        {
            scheduler.Start().Wait();
        }

        public async Task PopulateJobs(List<ScheduledJob> jobs, JobEngineServiceCore refererService)
        {
            foreach (var job in jobs)
            {
                await PopulateJob(job, refererService);
            }
        }

        public async Task PopulateJob(ScheduledJob model, JobEngineServiceCore refererService)
        {

            Type? jobType = GetJobTypeFromModel(model);

            if (jobType == null)
            {
                return;
            }

            IJobDetail? jobdetail = null;
            ITrigger? trigger = null;

            JobKey jobKey = JobKey.Create(GetJobIdentityKey(model));

            jobdetail = JobBuilder.Create().
               WithIdentity(jobKey)
               .OfType(jobType)
               .Build();

            jobdetail.JobDataMap[nameof(ScheduledJob)] = model;
            jobdetail.JobDataMap[nameof(ConfigurationService)] = configurationService;
            jobdetail.JobDataMap[nameof(JobEngineServiceCore)] = refererService;

            trigger = TriggerBuilder.Create()

            .WithIdentity(GetTriggerIdentityKey(model))
            .StartNow()
            .WithSimpleSchedule(builder => builder.RepeatForever()).WithCronSchedule(model.JobDetail.PeriodAsCron)
            .Build();

            await scheduler.ScheduleJob(jobdetail, trigger);
        }

        public async Task RemoveJobs(List<ScheduledJob> models)
        {
            foreach (var model in models)
            {
                JobKey jobKey = JobKey.Create(GetJobIdentityKey(model));
                await scheduler.UnscheduleJob(new TriggerKey(GetTriggerIdentityKey(model)));
                await scheduler.DeleteJob(jobKey, CancellationToken.None);
            }
        }

        public string GetTriggerIdentityKey(ScheduledJob model)
        {
            return $"{model.JobName}_{model.JobIdentity}_tik";
        }

        public string GetJobIdentityKey(ScheduledJob model)
        {
            return $"{model.JobName}_{model.JobIdentity}_jik";
        }

        public Type? GetJobTypeFromModel(ScheduledJob model)
        {
            Type? jobType = Type.GetType($"{model.JobName}, {model.Project}");

            if (jobType == null)
            {
                jobType = Type.GetType($"{model.Project}.{model.JobName}");
            }

            return jobType;
        }
    }
}
