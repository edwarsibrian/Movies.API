namespace Movies.Infrastructure.Settings
{
    public class SyncSettings
    {
        public int IntervalSeconds { get; set; }
        public int MaxRetries { get; set; }
        public int RetryDelaySeconds { get; set; }
    }
}
