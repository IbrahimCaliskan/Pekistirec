using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Shared
{
    public class reCaptchaCodebehind
    {
        public string reCaptchaPublicKey = Pekistirec.Engine.reCaptcha.reCaptchaCredentials.reCaptchaPublicKey;
    }
}