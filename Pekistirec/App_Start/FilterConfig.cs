﻿using System.Web;
using System.Web.Mvc;

namespace Pekistirec
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Pekistirec.Engine.EventLog.EventLogHelpers().GetEventLoggingFilter());
        }
    }
}
