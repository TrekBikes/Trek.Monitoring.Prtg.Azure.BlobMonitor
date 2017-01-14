namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    public class ChannelResults
    {
        public long TotalNumberOfBlockBlobs { get; set; } = 0;

        public long TotalContainerSizeInBytes { get; set; } = 0;

        public long TotalNumberOfMatchedBlockBlobs { get; set; } = 0;

        public long TotalContainerSizeOfMatchedBlobsInBytes { get; set; } = 0;
    }
}