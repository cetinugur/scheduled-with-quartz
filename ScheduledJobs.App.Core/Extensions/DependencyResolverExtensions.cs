using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.App.Core.Engines;
using ScheduledJobs.Core.Extensions;
using ScheduledJobs.Models;

namespace ScheduledJobs.App.Core.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddScheduledJobEngines(this IServiceCollection services)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appsettingsfile = string.IsNullOrEmpty(environment) ? "appsettings.json" : $"appsettings.{environment}.json";

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(appsettingsfile)
                .AddEnvironmentVariables()
                .Build();

            services
                    .AddSingleton<JobEngineAppCore>()
                    .AddSingleton(configuration)
                    .AddScheduledEngine();

            return services;
        }
    }
}
