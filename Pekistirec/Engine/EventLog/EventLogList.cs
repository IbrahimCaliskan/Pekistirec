using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pekistirec.Engine.EventLog
{
    public class EventLogList
    {
        private static List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity> _List 
            = new List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity>();

        public static List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity> Liste
        {
            get
            {
                return _List;
            }
        }

        public static void Add(AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity entity)
        {
            _List.Add(entity);

            if (_List.Count > Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_BUFFER_SIZE.Deger - 1)
            {
                foreach (var item in _List)
                {
                    while (_List.Where(i => i.RowKey == item.RowKey).Count() > 1)
                    {
                        var part = item.RowKey.Split('.');
                        item.RowKey = part[0] + "." + (int.Parse(part[1]) + 1).ToString();
                    }
                }
                
                var l = new List<AzureAPI.Implementation.EventLogAzure.EventLogAzureEntity>();
                _List.ForEach(x => l.Add(x));
                Task.Factory.StartNew(()=>
                    new AzureAPI.Implementation.EventLogAzure.EventLogAzureTable().InsertBatch(l)
                ) ;
                _List.Clear();
            }        
        }
    }
}
