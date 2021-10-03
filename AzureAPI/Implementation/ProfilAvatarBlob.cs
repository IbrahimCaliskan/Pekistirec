using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Implementation
{
    public class ProfilAvatarBlob
    {
        private AzureAPI.Storage.AzureBlob azureBlob;

        internal void CreateIfNotExist()
        {
            azureBlob.CreateIfNotExists();
        }

        public ProfilAvatarBlob()
        {
            azureBlob = new Storage.AzureBlob(Credentials.PROFIL_AVATAR_BLOB_CONATINER_NAME);
        }
        public bool Upload(string blobName, string path)
        {
            return azureBlob.Upload(blobName, path, true, false);
        }

        public  void Download(string blobName, string path)
        {
            azureBlob.Download(blobName, path);
        }
    }
}
