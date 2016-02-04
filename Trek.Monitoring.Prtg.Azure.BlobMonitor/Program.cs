using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitorArgs = Args.Configuration.Configure<MonitorArgs>().CreateAndBind(args);

            var account = CloudStorageAccount.Parse($@"DefaultEndpointsProtocol=https;AccountName={monitorArgs.StorageAccountName};AccountKey={monitorArgs.StorageAccountKey}");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(monitorArgs.ContainerName);

            var startTime = DateTime.UtcNow;
            long numOfBlockBlobs = 0;
            long numOfMatchedBlobs = 0;
            long containerSize = 0;
            long matchedBlobSize = 0;

            foreach (var blob in container.ListBlobs().OfType<CloudBlockBlob>())
            {
                numOfBlockBlobs++;
                containerSize += blob.Properties.Length;

                // Make sure it has a name, and it matches the pattern
                if (string.IsNullOrWhiteSpace(blob.Name) || !Regex.IsMatch(blob.Name, monitorArgs.BlobNamePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    continue;
                }

                // If ModifiedAfter is provided, make sure the file was modified after that time
                if (!monitorArgs.ModifiedAfter.Equals(TimeSpan.Zero)
                    && blob.Properties.LastModified.HasValue
                    && blob.Properties.LastModified.Value < startTime.Add(monitorArgs.ModifiedAfter.Negate()))
                {
                    continue;
                }

                // If ModifiedBefore is provided, make sure the file was modified before that time
                if (!monitorArgs.ModifiedBefore.Equals(TimeSpan.Zero)
                    && blob.Properties.LastModified.HasValue
                    && blob.Properties.LastModified.Value > startTime.Add(monitorArgs.ModifiedBefore.Negate()))
                {
                    continue;
                }

                numOfMatchedBlobs++;
                matchedBlobSize += blob.Properties.Length;
            }

            
            Console.WriteLine($@"{numOfBlockBlobs}:Number of Blobs in Container");
            Console.WriteLine($@"{containerSize}:Total Bytes of Blobs in Container");
            Console.WriteLine($@"{numOfMatchedBlobs}:Number of Matching Blobs in Container");
            Console.WriteLine($@"{matchedBlobSize}:Total Bytes of Matching Blobs in Container");
            Environment.Exit(0);
        }
    }
}
