using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Views.Dokuman
{
    public class YukleCodebehind
    {
        private DataLayer.DomainClasses.BaseDomainClasses.AspNetUser User;

        public string YukleFormControllerName;
        public string YukleFormActionName;
        public int DokumanBaslikMinLength = DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MINLENGTH;
        public int DokumanBaslikMaxLength = DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MAXLENGTH;
        public string jsOzelliklerArrayBuffer;
        public string jsDokumanUzantiArrayBuffer;
        public string jsDokumanUzantiVerticalBarSperatorBuffer;
        public bool RECAPTCHA_ENABLED = true;
        public bool reCaptchaGerekliMi = true;

        public YukleCodebehind(System.Security.Principal.IPrincipal User, Pekistirec.Models.DokumanModels.DokumanYukleViewModel model)
        {
            this.User = Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUser(User);

            var controllerAndActionName = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<Pekistirec.Controllers.DokumanController>(c => c.Yukle());
            YukleFormControllerName = controllerAndActionName.ControllerName;
            YukleFormActionName = controllerAndActionName.ActionName;

            jsOzelliklerArrayBuffer = Pekistirec.Engine.PekistirecBuffers.jsBuffers.OzelliklerArrayBuffer;
            jsDokumanUzantiArrayBuffer = Pekistirec.Engine.PekistirecBuffers.jsBuffers.DokumanUzantiArrayBuffer;
            jsDokumanUzantiVerticalBarSperatorBuffer = Pekistirec.Engine.PekistirecBuffers.jsBuffers.DokumanUzantiVerticalBarSperatorBuffer;

            RECAPTCHA_ENABLED = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_ENABLED.Deger;
            reCaptchaGerekliMi = model.reCaptchaGerekliMi;
        }
    }
}