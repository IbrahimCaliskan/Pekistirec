using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Engine.EventLog
{
    public class EventLogHelpers
    {
        private void Logging(object sender, EventLog.EventLogEntity eventLogEntity)
        {
            AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity entity =
                new AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity();

            entity.PartitionKey = eventLogEntity.TarihZaman.Substring(0, 10);
            entity.RowKey = eventLogEntity.TarihZaman.Substring(11, eventLogEntity.TarihZaman.Length - 11);
            entity.Action = eventLogEntity.Action;
            entity.Controller = eventLogEntity.Controller;
            entity.HttpBody = eventLogEntity.HttpBody;
            entity.HttpMethod = eventLogEntity.HttpMethod;
            entity.Mesaj = eventLogEntity.Mesaj;
            entity.RawUrl = eventLogEntity.RawUrl;            
            entity.Tip = eventLogEntity.Tip;
            entity.UrlReferrer = eventLogEntity.UrlReferrer;
            entity.UserIP = eventLogEntity.UserIP;
            entity.UserId = eventLogEntity.UserId;
            entity.UserName = eventLogEntity.UserName;

            EventLogList.Add(entity);
        }

        public ActionFilterAttribute GetEventLoggingFilter()
        {
            var filter = new EventLog.EventLogActionFilter();
            filter.Logging += Logging;
            return filter;
        }

        public static List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity> Son24SaatIcindekiLoglariGetirIPveLogTipineGore(string userIp, Pekistirec.Engine.EventLog.EventLogTipleri eventLogTipi)
        {
            var eventLog = Pekistirec.Engine.EventLog.EventLogList.Liste.Where(e => e.Tip == eventLogTipi.ToString()).ToList();
            var azureEventLog = AzureAPI.Implementation.EventLogAzure.EventLogAzureHelpers.Son24SaatIcindekiLoglariGetirIPveLogTipineGore
                            (userIp, eventLogTipi.ToString()).ToList();
            eventLog.AddRange(azureEventLog);
            return eventLog.Distinct().ToList();
        }

        public static List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity> Son1SaatIcindekiLoglariGetirIPveLogTipineGore(string userIp, Pekistirec.Engine.EventLog.EventLogTipleri eventLogTipi)
        {
            var eventLog = Pekistirec.Engine.EventLog.EventLogList.Liste.Where(e => e.Tip == eventLogTipi.ToString()).ToList();
            var azureEventLog = AzureAPI.Implementation.EventLogAzure.EventLogAzureHelpers.Son1SaatIcindekiLoglariGetirIPveLogTipineGore
                            (userIp, eventLogTipi.ToString()).ToList();
            eventLog.AddRange(azureEventLog);
            return eventLog.Distinct().ToList();
        }
    }
}