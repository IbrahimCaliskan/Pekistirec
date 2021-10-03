using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureAPI.Storage
{
    internal class AzureBlob
    {
        private CloudStorageAccount storageAccount { get; set; }
        private CloudBlobClient blobClient { get; set; }
        private CloudBlobContainer container { get; set; }

        private string containerName { get; set; }

        public AzureBlob(string containerName)
        {
            var azureStorage = new AzureStorage();
            storageAccount = azureStorage.storageAccount;
            this.containerName = containerName;

            this.blobClient = storageAccount.CreateCloudBlobClient();
            this.container = blobClient.GetContainerReference(containerName);            
        }

        internal void CreateIfNotExists()
        {
            container.CreateIfNotExists();
        }

        internal bool Upload(string blobName, string path, bool createContainerIfNotExist = true, bool publicContainer = true)
        {
            bool success = false;

            try
            {
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

                if (createContainerIfNotExist)
                {
                    container.CreateIfNotExists();
                }

                if (createContainerIfNotExist && publicContainer)
                {
                    container.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });
                }

                // Create or overwrite the "myblob" blob with contents from a local file.
                using (var fileStream = System.IO.File.OpenRead(path))
                {
                    blockBlob.UploadFromStream(fileStream);
                }

                success = true;
            }
            catch (Exception)
            {
                success = false;                
            }

            return success;            
        }

        internal  List<CloudBlockBlob> BlobList()
        {
            List<CloudBlockBlob> list = new List<CloudBlockBlob>();            

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    list.Add(blob);

                }
                //else if (item.GetType() == typeof(CloudPageBlob))
                //{
                //    CloudPageBlob pageBlob = (CloudPageBlob)item;

                //    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                //}
                //else if (item.GetType() == typeof(CloudBlobDirectory))
                //{
                //    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                //    Console.WriteLine("Directory: {0}", directory.Uri);
                //}
            }

            return list;
        }

        internal  void Download(string blobName, string path)
        {            

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(path))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }
    }
}
