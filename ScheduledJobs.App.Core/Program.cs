using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.Core.Extensions;

namespace ScheduledJobs.App.Core
{
    public static class Program
    {
        public static IServiceCollection AddScheduledJobEngines(this IServiceCollection services)
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
                    //.AddControllerJob(config["ProjectSettings:JobConfigControllerJobCronPeriod"])//TODO unutma
                    .AddScheduledEngine();
            return services;
        }
    }
}
