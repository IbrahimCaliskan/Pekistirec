using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Account
{
    public class FbCallbackCodebehind
    {
        public void ParameterTampering(System.Web.Mvc.MvcHtmlString antiForgeryToken, Pekistirec.Engine.FacebookAPI.FacebookUser FbUser,string UserIp)
        {
            Pekistirec.Engine.ParameterTampering.ParameterTamperingHelpers.Ekle(antiForgeryToken, FbUser, UserIp);
        }
    }
}