namespace ScheduledJobs.Core.Models
{
    public class OperationResult
    {
        public bool HasError { get; set; } = false;
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }
        public Exception? Exception { get; set; } = null;
    }
}
