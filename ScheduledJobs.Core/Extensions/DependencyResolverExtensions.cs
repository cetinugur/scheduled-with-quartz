using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using ScheduledJobs.Core.Jobs;
using ScheduledJobs.Core.Services;

namespace ScheduledJobs.Core.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(new ConfigurationService(config));
            return services;
        }

        public static IServiceCollection AddScheduledEngine(this IServiceCollection services)
        {
            services.AddSingleton<JobEngineCoreService>();
            services.AddSingleton<StdSchedulerFactory>();
            services.AddSingleton<ConfigControllerJob>();
            return services;
        }

        public static IServiceCollection AddControllerJob(this IServiceCollection services, string cronperiod)
        {
            JobEngineBaseService.UserControllerJob = true;
            JobEngineBaseService.CronPeriod = cronperiod;
            return services;
        }
    }
}
