using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Implementation.EventLogAzure
{
    public static class EventLogAzureHelpers
    {
        public static IQueryable<EventLogAzureEntity> Son24SaatIcindekiLoglariGetirIPveLogTipineGore(string UserIP, string LogTipi)
        {
            //DateTime now = DateTime.Now.ToUniversalTime();
            DateTime now = DateTime.Now;            
            return from entity in new EventLogAzureTable().azureTable.cloudTableClient.GetTableServiceContext()
                            .CreateQuery<EventLogAzureTableEntity>(EventLogAzureCredentials.EVENTLOG_TABLE_NAME)
                   where (
                   entity.UserIP.Equals(UserIP)
                   &&
                   entity.PartitionKey ==
                       String.Format
                           ("{0}.{1}.{2}",
                               (now.Day < 10) ? "0" + now.Day.ToString() : now.Day.ToString(),
                               (now.Month < 10) ? "0" + now.Month.ToString() : now.Month.ToString(),
                               now.Year
                           )
                   &&
                   entity.Tip.CompareTo(LogTipi) >= 0
                   )
                   select new EventLogAzureEntity
                   {
                       Action = entity.Action,
                       Controller = entity.Controller,
                       HttpBody = entity.HttpBody,
                       HttpMethod = entity.HttpMethod,
                       Mesaj = entity.Mesaj,
                       PartitionKey = entity.PartitionKey,
                       RawUrl = entity.RawUrl,
                       RowKey = entity.RowKey,
                       Tip = entity.Tip,
                       UrlReferrer = entity.UrlReferrer,
                       UserId = entity.UserId,
                       UserIP = entity.UserIP,
                       UserName = entity.UserName
                   };

        }



        public static IQueryable<EventLogAzureEntity> Son1SaatIcindekiLoglariGetirIPveLogTipineGore(string UserIP, string LogTipi)
        {
            //DateTime now = DateTime.Now.ToUniversalTime();
            DateTime now = DateTime.Now;
            return from entity in new EventLogAzureTable().azureTable.cloudTableClient.GetTableServiceContext()
                            .CreateQuery<EventLogAzureTableEntity>(EventLogAzureCredentials.EVENTLOG_TABLE_NAME)
                           where (
                           entity.UserIP.Equals(UserIP) 
                           &&
                           entity.PartitionKey ==
                               String.Format
                                   ("{0}.{1}.{2}",
                                       (now.Day < 10) ? "0" + now.Day.ToString() : now.Day.ToString(),
                                       (now.Month < 10) ? "0" + now.Month.ToString() : now.Month.ToString(),
                                       now.Year
                                   )
                           &&
                           entity.RowKey.CompareTo((now.Hour + 1).ToString()) <= 0 && entity.RowKey.CompareTo((now.Hour - 1).ToString()) >= 0
                           &&
                           entity.Tip.CompareTo(LogTipi) >= 0
                           )
                           select new EventLogAzureEntity
                           {
                               Action = entity.Action,
                               Controller = entity.Controller,
                               HttpBody = entity.HttpBody,
                               HttpMethod = entity.HttpMethod,
                               Mesaj = entity.Mesaj,
                               PartitionKey = entity.PartitionKey,
                               RawUrl = entity.RawUrl,
                               RowKey = entity.RowKey,
                               Tip = entity.Tip,
                               UrlReferrer = entity.UrlReferrer,
                               UserId = entity.UserId,
                               UserIP = entity.UserIP,
                               UserName = entity.UserName
                           };        

        }
    }
}
