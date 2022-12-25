using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Core.Jobs;
using ScheduledJobs.Core.Services;
using ScheduledJobs.Data.DependencyResolverExtensions;

namespace ScheduledJobs.Core.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddScheduledEngine(this IServiceCollection services)
        {
            services.AddSingleton<IJobService,JobService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IJobEngineServiceCore,JobEngineServiceCore>();
            services.AddSingleton<IModelService, ModelService>();
            services.AddSingleton<StdSchedulerFactory>();
            services.AddSingleton<ConfigControllerJob>();
            services.AddDataService();
            return services;
        }
    }
}
