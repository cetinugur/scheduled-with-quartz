using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.Data.Interfaces;
using ScheduledJobs.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobs.Data.DependencyResolverExtensions
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
