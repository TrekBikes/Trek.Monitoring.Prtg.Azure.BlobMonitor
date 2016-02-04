using System;

namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    public class MonitorArgs
    {
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public string ContainerName { get; set; }
        public string BlobNamePattern { get; set; } = ".*";

        public TimeSpan ModifiedAfter { get; set; } = TimeSpan.Zero;

        public TimeSpan ModifiedBefore { get; set; } = TimeSpan.Zero;

    }
}