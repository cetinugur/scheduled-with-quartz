using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using ScheduledJobs.Core.Jobs;
using ScheduledJobs.Core.Services;
using ScheduledJobs.Data.DependencyResolverExtensions;
using ScheduledJobs.Models;

namespace ScheduledJobs.Core.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddScheduledEngine(this IServiceCollection services)
        {
            services.AddSingleton<ConfigurationService, ConfigurationService>();
            services.AddSingleton<JobEngineCoreService>();
            services.AddSingleton<StdSchedulerFactory>();
            services.AddSingleton<ConfigControllerJob>();
            services.AddDataService();
            return services;
        }

        public static IServiceCollection AddConfigControllerJob(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(Projectsettings)).Get<Projectsettings?>();

            JobEngineBaseService.AddConfigControllerJob = true;
            JobEngineBaseService.ConfigControllerJobCronPeriod = options?.ConfigControllerJobCronPeriod;
            return services;
        }
    }
}
