using System;
using PowerArgs;

namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    public class MonitorArgs
    {
        [ArgShortcut("N"),
            ArgRequired,
            ArgDescription("Required: The name of the storage account")]
        public string StorageAccountName { get; set; }

        [ArgShortcut("K"), 
            ArgRequired,
            ArgDescription("Required: The storage account's key")]
        public string StorageAccountKey { get; set; }

        [ArgShortcut("C"), 
            ArgDescription("Optional: The name of the container to look in.  If not provided will look into the most recently modified container")]
        public string ContainerName { get; set; }

        [ArgShortcut("B"), 
            ArgDefaultValue(".*"),
            ArgDescription("Optional: A RegEx to describe the pattern of name of the blobs to look for"),
            ArgExample("^log-.*", "All blobs starting with log-")]
        public string BlobNamePattern { get; set; }

        [ArgShortcut("S"),
            ArgDefaultValueAttribute(true),
            ArgDescription("Optional: Should the results include blobs found in virtual directories under the container")]
        public bool MatchInSubdirectories { get; set; }

        [ArgShortcut("MA"),
            ArgDescription("Optional: A TimeSpan string descibing how long ago to look back")]
        public TimeSpan ModifiedAfter { get; set; } = TimeSpan.Zero;

        [ArgShortcut("MB"), 
            ArgDescription("Optional: A TimeSpan string descibing how long ago to stop looking")]
        public TimeSpan ModifiedBefore { get; set; } = TimeSpan.Zero;

    }
}