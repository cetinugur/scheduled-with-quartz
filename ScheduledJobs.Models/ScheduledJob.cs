namespace ScheduledJobs.Models
{
    public class ScheduledJob
    {
        public int Id { get; set; }
        public string? Project { get; set; }
        public string? JobName { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }

        public List<ScheduledJobDetail>? JobDetails { get; set; }

        public ScheduledJobDetail? JobDetail
        {
            get
            {
                if (JobDetails != null && JobDetails.Count > 0)
                    return JobDetails?.FirstOrDefault();
                else
                    return new ScheduledJobDetail();
            }
        }

        public string JobIdentity
        {
            get
            {
                string result = $"{Id}_{JobDetail?.Id}";

                return result;
            }
        }
    }
}
