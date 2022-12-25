namespace ScheduledJobs.Models
{
    public class OperationResult
    {
        public bool IsSucceed { get; set; } = false;
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }
        public Exception? Exception { get; set; } = null;
    }
}
