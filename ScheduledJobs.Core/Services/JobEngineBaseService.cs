using Quartz.Impl;
using Quartz;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Services
{
    public abstract class JobEngineBaseService
    {
        public static bool UserControllerJob { get; set; } = false;
        public static string CronPeriod { get; set; } = "0 0/20 * 1/1 * ? *";

        protected IScheduler scheduler;

        protected ConfigurationService configurationService { get; set; }

        public JobEngineBaseService(StdSchedulerFactory stdSchedulerFactory, ConfigurationService configurationservice)
        {
            this.configurationService = configurationservice;
            this.scheduler = stdSchedulerFactory.GetScheduler().Result;
        }

        public async Task PopulateJobs(List<ScheduledJob> jobs)
        {
            foreach (var job in jobs)
            {
                await PopulateJob(job);
            }
        }

        public async Task PopulateJob(ScheduledJob model)
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
            jobdetail.JobDataMap[nameof(ConfigurationService)] = this.configurationService;
            jobdetail.JobDataMap[nameof(JobEngineBaseService)] = this;

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

        public async Task ReScheduleJobs(List<ScheduledJob> models)
        {
            foreach (var job in models)
            {
                await ReScheduleJob(job);
            }
        }

        public async Task ReScheduleJob(ScheduledJob model)
        {
            var exTrigger = scheduler.GetTrigger(new TriggerKey(GetTriggerIdentityKey(model)));

            if (exTrigger.Result == null)
            {
                await PopulateJob(model);
                return;
            }

            ITrigger? trigger = null;

            trigger = TriggerBuilder.Create()

            .WithIdentity(GetTriggerIdentityKey(model))
            .StartNow()
            .WithSimpleSchedule(builder => builder.RepeatForever()).WithCronSchedule(model.JobDetail.PeriodAsCron)
            .Build();

            var exTriggerKey = exTrigger.Result.Key;

            var jobdetail = (await scheduler.GetJobDetail(new JobKey(GetJobIdentityKey(model))).ConfigureAwait(false));

            await scheduler.RescheduleJob(exTriggerKey, trigger);
        }

        private string GetTriggerIdentityKey(ScheduledJob model)
        {
            return $"{model.JobName}_{model.JobIdentity}_tik";
        }

        private string GetJobIdentityKey(ScheduledJob model)
        {
            return $"{model.JobName}_{model.JobIdentity}_jik";
        }

        private Type? GetJobTypeFromModel(ScheduledJob model)
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
