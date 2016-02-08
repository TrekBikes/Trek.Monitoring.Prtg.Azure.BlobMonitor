using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PowerArgs;

namespace Trek.Monitoring.Prtg.Azure.BlobMonitor
{
    class Program
    {
        public static readonly DateTime StartTime = DateTime.UtcNow;

        static void Main(string[] args)
        {
            MonitorArgs monitorArgs = null;

            try
            {
                monitorArgs = Args.Parse<MonitorArgs>(args);
            }
            catch (ArgException ex)
            {
                Console.WriteLine(Environment.NewLine + ex.Message + Environment.NewLine);
                ArgUsage.GenerateUsageFromTemplate<MonitorArgs>().WriteLine();
                Environment.Exit((int)PrtgEnvironmentExitCode.SystemError); // System Error
            }

            int numOfBlockBlobs = 0;
            int numOfMatchedBlobs = 0;
            long containerSize = 0;
            long matchedBlobSize = 0;

            foreach (var blob in GetContainer(monitorArgs).ListCloudBlockBlobs(monitorArgs.MatchInSubdirectories))
            {
                numOfBlockBlobs++;
                containerSize += blob.Properties.Length;

                // Make sure it has a name, and it matches the pattern
                if (string.IsNullOrWhiteSpace(blob.Name) || !Regex.IsMatch(blob.Name, monitorArgs.BlobNamePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    continue;
                }

                // If the blob as a LastModified date, then run it through the process -- if it doesn't we will
                // just include it in the results (not perfect, but edge case anyway)
                if (blob.Properties.LastModified.HasValue)
                {
                    // If ModifiedAfter is provided, make sure the file was modified after that time
                    if (!monitorArgs.ModifiedAfter.Equals(TimeSpan.Zero)
                        && blob.Properties.LastModified.Value < StartTime.Add(monitorArgs.ModifiedAfter.Negate()))
                    {
                        continue;
                    }

                    // If ModifiedBefore is provided, make sure the file was modified before that time
                    if (!monitorArgs.ModifiedBefore.Equals(TimeSpan.Zero)
                        && blob.Properties.LastModified.Value > StartTime.Add(monitorArgs.ModifiedBefore.Negate()))
                    {
                        continue;
                    }
                }


                numOfMatchedBlobs++;
                matchedBlobSize += blob.Properties.Length;
            }

            
            Console.WriteLine($@"{numOfBlockBlobs}:Number of Blobs in Container");
            Console.WriteLine($@"{containerSize}:Total Bytes of Blobs in Container");
            Console.WriteLine($@"{numOfMatchedBlobs}:Number of Matching Blobs in Container");
            Console.WriteLine($@"{matchedBlobSize}:Total Bytes of Matching Blobs in Container");
            Environment.Exit((int)PrtgEnvironmentExitCode.Ok);
        }

        private static CloudBlobContainer GetContainer(MonitorArgs monitorArgs)
        {
            var account = CloudStorageAccount.Parse($@"DefaultEndpointsProtocol=https;AccountName={monitorArgs.StorageAccountName};AccountKey={monitorArgs.StorageAccountKey}");
            var client = account.CreateCloudBlobClient();
            return string.IsNullOrWhiteSpace(monitorArgs.ContainerName) ? 
                client.ListContainers().OrderByDescending(c => c.Properties.LastModified.GetValueOrDefault(DateTime.MinValue)).First() 
                : client.GetContainerReference(monitorArgs.ContainerName);
        }
    }
}
