using ScheduledJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobs.Data.Interfaces
{
    public interface IDataService
    {
        IEnumerable<ScheduledJob> GetJobs();
    }
}
