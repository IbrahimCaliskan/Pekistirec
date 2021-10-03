using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec
{
    public static class EventLogExtensions
    {
        public static void SetEventLogControllerEnabledValue(this Controller controller, bool EventLogEnabled)
        {
            controller.ViewBag.EVENTLOG_CONTROLLER_ENABLED = EventLogEnabled;
        }


        public static void SetEventLogMessageValue(this Controller controller, string Message)
        {
            controller.ViewBag.EVENTLOG_MESSAGE = Message;
        }

        public static void SetEventLogTipValue(this Controller controller,  Pekistirec.Engine.EventLog.EventLogTipleri EventLogTip)
        {
            controller.ViewBag.EVENTLOG_TIP = EventLogTip;
        }
    }
}