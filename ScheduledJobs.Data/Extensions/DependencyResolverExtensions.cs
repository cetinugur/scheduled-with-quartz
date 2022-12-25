using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Data.Services;

namespace ScheduledJobs.Data.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddSingleton<IDataService, DummyDataService>();
            return services;
        }
    }
}
