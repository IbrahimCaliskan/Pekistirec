using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Storage
{
    internal class AzureTable<T, E> where T : ITableEntity
    {
        internal CloudTableClient cloudTableClient { get; set; }
        internal TableServiceContext tableServiceContext { get; set; }
        private CloudStorageAccount cloudStorageAccount { get; set; }
        private CloudTable cloudTable { get; set; }
        private string tableName { get; set; }

        internal AzureTable(string tableName)
        {
            this.tableName = tableName;

            var azureStorage = new AzureStorage();            

            this.cloudStorageAccount = azureStorage.storageAccount;

            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            //tableServiceContext = cloudTableClient.GetTableServiceContext();
            cloudTable = cloudTableClient.GetTableReference(tableName);
        }

        internal AzureTable(AzureStorage azureStorage)
        {
            if (azureStorage == null)
                azureStorage = new AzureStorage();

            this.cloudStorageAccount = azureStorage.storageAccount;

            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTable = cloudTableClient.GetTableReference(tableName);
        }

        internal bool CreateIfNotExists()
        {
            // Create the table if it doesn't exist.
            return cloudTable.CreateIfNotExists();
        }

        internal bool DeleteTable()
        {
            // Create the table if it doesn't exist.
            return cloudTable.DeleteIfExists();
        }

        internal void Insert(T entity)
        {

            TableOperation insertOperation = TableOperation.Insert(entity);
            cloudTable.Execute(insertOperation);

        }

        internal void InsertBatch(List<T> entityList)
        {            
            ILookup<string, T> insertList = entityList.ToLookup(x => x.PartitionKey, x => x);
            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach (var lookUp in insertList)
            {
                foreach (var entity in lookUp)
                {
                    batchOperation.InsertOrReplace(entity);
                    if (batchOperation.Count >= 99)
                    {
                        cloudTable.ExecuteBatch(batchOperation);
                        batchOperation.Clear();
                    }
                }

                if (batchOperation.Count > 0)
                {
                        cloudTable.ExecuteBatch(batchOperation);
                        batchOperation.Clear();
                   
                }
            }
        }

        internal IQueryable<E> Query()
        {
            return tableServiceContext.CreateQuery<E>(tableName).Select(e => e);
        }
    }
}
