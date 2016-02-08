using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    public static class CloudBlobContainerExtensions
    {
        public static IEnumerable<CloudBlockBlob> ListCloudBlockBlobs(this CloudBlobContainer self, bool includeSubDirectories)
        {
            return self.ListBlobs(useFlatBlobListing: includeSubDirectories).OfType<CloudBlockBlob>();
        }
    }
}