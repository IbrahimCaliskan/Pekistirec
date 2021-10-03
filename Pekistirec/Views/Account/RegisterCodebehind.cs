using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Account
{
    public class RegisterCodebehind
    {
        public string FbLoginUrl = Pekistirec.Engine.FacebookAPI.FacebookAPIHelpers.LoginURL;
        public string GoogleLoginUrl = GoogleAPI.Utils.AuthorizationUrl(true);
        public bool RECAPTCHA_ENABLED = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_ENABLED.Deger;
    }
}