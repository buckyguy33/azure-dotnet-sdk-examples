using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string containerName = "container2";
            string blobName = "test2";

            string storageconnection = System.Configuration.ConfigurationSettings.AppSettings.Get("StorageConnectionString");
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse(storageconnection);

            CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            using (var fileStream = System.IO.File.OpenRead(@"C:\Users\van1247\Documents\helloworld.txt"))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            

            ListAttributes(container);
            SetMetadata(container);
            ListMetadata(container);
            Console.ReadKey();
        }

        static void ListAttributes(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("Container name " + container.StorageUri.PrimaryUri.ToString());
            Console.WriteLine("Last modified " + container.Properties.LastModified.ToString());
        }

        static void SetMetadata(CloudBlobContainer container)
        {
            container.Metadata.Clear();
            container.Metadata.Add("author", "David Vandermause");
            container.Metadata["authoredOn"] = "Dec 3, 2018";
            container.SetMetadata();
        }

        static void ListMetadata(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("Metadata:\n");
            foreach (var item in container.Metadata)
            {
                Console.WriteLine("Key " + item.Key);
                Console.WriteLine("Value " + item.Value + "\n\n");
            }
        }
    }
}
