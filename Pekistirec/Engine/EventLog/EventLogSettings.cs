using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.EventLog
{
    public class EventLogSettings
    {
        public static bool EVENTLOG_ENABLED
        {
            get
            {
                return Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_ENABLED.Deger;
            }
        }
    }
}