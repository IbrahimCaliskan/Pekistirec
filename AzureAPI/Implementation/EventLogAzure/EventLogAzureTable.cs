using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Implementation.EventLogAzure
{
    public class EventLogAzureTable
    {
        internal AzureAPI.Storage.AzureTable<EventLogAzureTableEntity, EventLogAzureEntity> azureTable;

        internal void CreateIfNotExist()
        {
            azureTable.CreateIfNotExists();
        }        

        public EventLogAzureTable()
        {
            azureTable = new Storage.AzureTable<EventLogAzureTableEntity, EventLogAzureEntity>(EventLogAzure.EventLogAzureCredentials.EVENTLOG_TABLE_NAME);
        }

        public IQueryable<EventLogAzureEntity> Query()
        {
            return azureTable.Query();
        }

        public void Insert(EventLogAzureEntity entity)
        {
            azureTable.Insert(new EventLogAzureTableEntity(entity));
        }

        public void InsertBatch(List<EventLogAzureEntity> entityList)
        {
            var list = new List<EventLogAzureTableEntity>();
            foreach (var item in entityList)
            {
                list.Add(new EventLogAzureTableEntity(item));
            }
            azureTable.InsertBatch(list);            
        }
    }
}
