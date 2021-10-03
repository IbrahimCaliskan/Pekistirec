using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekistirec.Engine.EventLog
{  
    public class EventLogEntity : EventArgs, IEventLogEntity
    {
        public string TarihZaman { get; set; }
        public string UserIP { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Tip { get; set; }
        public string Mesaj { get; set; }
        public string HttpMethod { get; set; }
        public string RawUrl { get; set; }
        public string UrlReferrer { get; set; }
        public string HttpBody { get; set; }
    }
}
