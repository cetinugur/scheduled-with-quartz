using Quartz;
using ScheduledJobs.App.Core.Interfaces;
using ScheduledJobs.App.MWImpExample.Models;
using ScheduledJobs.App.MWImpExample.ServiceClients;
using ScheduledJobs.Core.Services;
using ScheduledJobs.Models;

namespace ScheduledJobs.App.MWImpExample.Jobs
{
    public class MiddleWareExampleJob : IAppJobBase
    {
        public ScheduledJob JobModel { get; set; }
        public ConfigurationService ConfigService { get; set; }
        protected JobEngineServiceCore JobEngineService { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}{nameof(MiddleWareExampleJob)} trigged.");

            DummyPostServiceClient client = new();
            Guid processId = Guid.NewGuid();
            try
            {
                PopulateJobDataMap(context);
                var dataList = GetDataFromSource<List<GetFromAServiceModel>>();

                foreach (var item in dataList)
                {
                    var contract = CreateContract<SendToAnotherServiceModel, GetFromAServiceModel>(item);
                    var operationResult = client.SendData(contract);

                    if (operationResult.IsSucceed)
                    {
                        Console.WriteLine($"{DateTime.UtcNow} data sent. {nameof(processId)}:{processId}");
                    }
                    else
                    {
                        Console.WriteLine(operationResult.Exception.Message);
                    }

                }

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            Console.WriteLine($"{DateTime.Now}{nameof(MiddleWareExampleJob)} ended.");
            return Task.CompletedTask;
        }
        public void PopulateJobDataMap(IJobExecutionContext context)
        {
            JobModel = (ScheduledJob)context.JobDetail.JobDataMap[nameof(ScheduledJob)];
            ConfigService = (ConfigurationService)context.JobDetail.JobDataMap[nameof(ConfigurationService)];
            JobEngineService = (JobEngineServiceCore)context.JobDetail.JobDataMap[nameof(JobEngineServiceCore)];
        }

        public T CreateContract<T, M>(M model) where T : class
        {
            var item = model as GetFromAServiceModel;

            var result = new SendToAnotherServiceModel
            {
                Id = item.Id,
                Name = item.Name,
                Integrator = JobModel.Description
            };

            return (T)Convert.ChangeType(result, typeof(T));
        }


        public T GetDataFromSource<T>() where T : class
        {
            // Get data from a custom datasource...
            /*JobModel nesnesi farklı data içerecek şekilde genişletilebilir, bu data burada tüketilebilir.
             * 
             * 
             */
            var result = new List<GetFromAServiceModel>()
            {
                new GetFromAServiceModel{Id = 1,Name ="1" },
                new GetFromAServiceModel{Id = 2,Name ="2" },
            };

            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
