using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Account
{
    public class GoogleCallbackCodebehind
    {
        public void ParameterTampering(System.Web.Mvc.MvcHtmlString antiForgeryToken, GoogleAPI.GoogleAPICarrier googleUser, string UserIp)
        {
            Pekistirec.Engine.ParameterTampering.ParameterTamperingHelpers.Ekle(antiForgeryToken, googleUser, UserIp);
        }
    }
}