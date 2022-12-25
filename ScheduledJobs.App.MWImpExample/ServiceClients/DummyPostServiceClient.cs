using ScheduledJobs.App.MWImpExample.Models;
using ScheduledJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScheduledJobs.App.MWImpExample.ServiceClients
{
    public class DummyPostServiceClient
    {
        public OperationResult SendData(SendToAnotherServiceModel model)
        {
			OperationResult result = new() { IsSucceed = true };
			try
			{
				result.RequestJson = JsonSerializer.Serialize(model);
				result.ResponseJson = "some response as json";
            }
			catch (Exception exp)
			{
				result.Exception = exp;
				result.IsSucceed = false;
			}

			return result;
        }
    }
}
