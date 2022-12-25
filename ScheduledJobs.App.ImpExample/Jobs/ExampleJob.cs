using Quartz;
using ScheduledJobs.Core.Interfaces;
using ScheduledJobs.Core.Models;
using ScheduledJobs.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobs.App.ImpExample.Jobs
{
    public class ExampleJob : ISchedulerJob
    {
        public ScheduledJobModel? JobModel { get; set; }
        public ConfigurationService? ConfigurationService { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobModel = (ScheduledJobModel)context.JobDetail.JobDataMap[nameof(ScheduledJobModel)];
                ConfigurationService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(ConfigurationService)];

            }
            catch (Exception exp)
            {

                Console.WriteLine(exp.ToString());
            }
        }
    }
}
