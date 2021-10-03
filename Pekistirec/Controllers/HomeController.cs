using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Controllers
{
    public class HomeController : Controller
    {
        //var identityHelper = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this);
        //identityHelper.SignOut();
        //DataLayer.DataLayerHelpers.YeniVeritabani();

        //var adminUser = new Engine.AspNetIdentity.ApplicationUser() { UserName = "admin" };
        //var identityResult = identityHelper.CreateUser(adminUser, "a");
        //identityHelper.RolleriGuncelle();
        //identityHelper.AddToRole(adminUser.Id, Pekistirec.Engine.AspNetIdentity.Roller.Admin.ToString());


        //.....
        // GET: Home        
        public ActionResult Index()
        {




            if (User.Identity.IsAuthenticated) return RedirectToAction(Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari.ProfilControllerVeActionlari.Index, Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari.ProfilControllerVeActionlari.ControllerName);
            return View();
        }
    }
}