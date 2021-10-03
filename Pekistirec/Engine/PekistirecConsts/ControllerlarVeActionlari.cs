using Pekistirec.Controllers;
using Pekistirec.Engine.Toolbox.GenelToolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari
{
    public static class HomeControllerVeActionlari
    {        
        public static string ControllerName = GenelTools.GetControllerAndActionName<HomeController>(c => c.Index()).ControllerName;

        public static string Index = GenelTools.GetControllerAndActionName<HomeController>(c => c.Index()).ActionName;
    }

    public static class AdminControllerVeActionlari
    {
        public static string ControllerName = GenelTools.GetControllerAndActionName<AdminController>(c => c.Index()).ControllerName;

        public static string Index = GenelTools.GetControllerAndActionName<AdminController>(c => c.Index()).ActionName;

        public static string KullaniciAyrinti = GenelTools.GetControllerAndActionName<AdminController>(c => c.KullaniciAyrinti("")).ActionName;

        public static string KullaniciAra = GenelTools.GetControllerAndActionName<AdminController>(c => c.KullaniciAra("")).ActionName;
    }

    public static class ProfilControllerVeActionlari
    {
        public static string ControllerName = GenelTools.GetControllerAndActionName<ProfilController>(c => c.Index()).ControllerName;
        
        public static string Index = GenelTools.GetControllerAndActionName<ProfilController>(c => c.Index()).ActionName;
    }

    public static class AccountControllerVeActionlari
    {
        public static string ControllerName = GenelTools.GetControllerAndActionName<AccountController>(c => c.Index()).ControllerName;

        public static string Login = GenelTools.GetControllerAndActionName<AccountController>(c => c.Login("")).ActionName;

        public static string Manage = GenelTools.GetControllerAndActionName<AccountController>(c => c.Manage(Pekistirec.Controllers.AccountController.ManageMessageId.Error)).ActionName;
    }

    public static class DokumanControllerVeActionlari
    {
        public static string ControllerName = GenelTools.GetControllerAndActionName<DokumanController>(c => c.Index(1,"","",false)).ControllerName;

        public static string Index = GenelTools.GetControllerAndActionName<DokumanController>(c => c.Index(1,"","",false)).ActionName;

        public static string g = GenelTools.GetControllerAndActionName<DokumanController>(c => c.g("")).ActionName;

        public static string Duzenle = GenelTools.GetControllerAndActionName<DokumanController>(c => c.Duzenle("","")).ActionName;

        public static string Yuklediklerim = GenelTools.GetControllerAndActionName<DokumanController>(c => c.Yuklediklerim()).ActionName;
    }
}