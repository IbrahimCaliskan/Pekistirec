using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace Pekistirec
{
    public static class ControllerExtensions
    {        
        public static string GetUserId(this System.Web.Mvc.Controller controller)
        {            
            return Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUserId(controller.User);
        }

        public static bool IsInRole(this System.Web.Mvc.Controller controller, Pekistirec.Engine.AspNetIdentity.Roller role)
        {
            var aspnetIdentity = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(controller);
            return aspnetIdentity.IsInRole(controller.GetUserId(), role.ToString());
        }

        public static DataLayer.DomainClasses.BaseDomainClasses.AspNetUser GetUser(this System.Web.Mvc.Controller controller, bool VeritabanindanGuncelle = false, DataLayer.UnitOfWorks.IBaseUnitOfWork uow = null)
        {
            return Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUser(controller.User, VeritabanindanGuncelle, uow);
        }

        public static string GetRootURL(this System.Web.Mvc.Controller controller)
        {
            return string.Format("{0}://{1}{2}", controller.Request.Url.Scheme, controller.Request.Url.Authority, controller.Url.Content("~"));
        }

        public static string GetUserIP(this System.Web.Mvc.Controller controller)
        {
            return controller.HttpContext.Request.UserHostAddress;
        }
    }
}