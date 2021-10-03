using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace Pekistirec.Engine.EventLog
{
    public class EventLogActionFilter : ActionFilterAttribute
    {
        public event EventHandler<EventLogEntity> Logging;

        protected virtual void OnLoggingEvent(EventLogEntity logEntity)
        {
            var del = Logging as EventHandler<EventLogEntity>;
            if (del != null)
            {
                del(this, logEntity);
            }
        }

        private string GetUserId(IPrincipal User)
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Identity.GetUserId();
            }

            return null;
        }

        private string CiftHaneYap(int sayi)
        {
            if (sayi < 10)
            {
                return String.Format("0{0}", sayi);
            }
            else
            {
                return sayi.ToString();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (EventLogSettings.EVENTLOG_ENABLED)
            {
                bool EVENTLOG_CONTROLLER_ENABLED = true;
                if (filterContext.Controller.ViewBag.EVENTLOG_CONTROLLER_ENABLED != null)
                {
                    bool EVENTLOG_CONTROLLER_ENABLED_ConvertDeneme = bool.TryParse(Convert.ToString(filterContext.Controller.ViewBag.EVENTLOG_ENABLE), out EVENTLOG_CONTROLLER_ENABLED);
                    if (EVENTLOG_CONTROLLER_ENABLED_ConvertDeneme == false)
                        EVENTLOG_CONTROLLER_ENABLED = true;
                }

                if (EVENTLOG_CONTROLLER_ENABLED)
                {
                    DateTime n = DateTime.Now;

                    EventLogEntity logEntity = new EventLogEntity()
                    {
                        Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower().Replace('ı', 'i'),
                        Action = filterContext.ActionDescriptor.ActionName.ToLower().Replace('ı', 'i'),
                        HttpMethod = filterContext.HttpContext.Request.HttpMethod.ToLower(),
                        HttpBody = new System.IO.StreamReader(filterContext.HttpContext.Request.InputStream).ReadToEnd(),
                        RawUrl = filterContext.HttpContext.Request.RawUrl.ToLower(),
                        TarihZaman = String.Format("{0}.{1}.{2} {3}:{4}:{5}.{6}",
                                                                                CiftHaneYap(n.Day),
                                                                                CiftHaneYap(n.Month),
                                                                                n.Year,
                                                                                CiftHaneYap(n.Hour),
                                                                                CiftHaneYap(n.Minute),
                                                                                CiftHaneYap(n.Second),
                                                                                n.Millisecond),
                        UserIP = filterContext.HttpContext.Request.UserHostAddress,
                        UserName = (filterContext.HttpContext.User.Identity.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "",
                        UrlReferrer = (filterContext.HttpContext.Request.UrlReferrer != null) ? filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri : "",
                        Mesaj = (filterContext.Controller.ViewBag.EVENTLOG_MESSAGE != null) ? filterContext.Controller.ViewBag.EVENTLOG_MESSAGE : "",
                        Tip = (filterContext.Controller.ViewBag.EVENTLOG_TIP != null) ? Convert.ToString(filterContext.Controller.ViewBag.EVENTLOG_TIP) : "",
                        UserId = (filterContext.HttpContext.User.Identity.IsAuthenticated) ? GetUserId(filterContext.HttpContext.User) : "",
                    };

                    OnLoggingEvent(logEntity);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
