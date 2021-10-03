using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AzureAPI.Storage
{
    internal class AzureStorage
    {
        internal CloudStorageAccount storageAccount { get; set; }

        private static string _ConnectionString = null;
        private static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_ConnectionString))
                {
                    _ConnectionString = WebConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
                }
                return _ConnectionString;
            }
        }

        internal AzureStorage()
        {
            storageAccount = CloudStorageAccount.Parse(ConnectionString);            
        }
    }
}
