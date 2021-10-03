using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.reCaptcha
{
    public class reCaptchaSettings
    {
        public static bool RECAPTCHA_ENABLED
        {
            get
            {
                return Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_ENABLED.Deger;
            }
        }
    }
}