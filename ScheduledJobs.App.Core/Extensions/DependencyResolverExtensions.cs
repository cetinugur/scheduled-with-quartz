using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.App.Core.Engines;
using ScheduledJobs.Core.Extensions;

namespace ScheduledJobs.App.Core.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddScheduledJobEngines(this IServiceCollection services, bool withConfigControllerJob)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appsettingsfile = string.IsNullOrEmpty(environment) ? "appsettings.json" : $"appsettings.{environment}.json";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(appsettingsfile)
                .AddEnvironmentVariables()
                .Build();

            services
                    .AddSingleton<ScheduledJobsEngine>()
                    .AddBusinessServices(config)
                    .AddScheduledEngine();

            if (withConfigControllerJob)
            {
                services.AddControllerJob(config["ProjectSettings:ConfigControllerJobCronPeriod"]);
            }

            return services;
        }
    }
}
