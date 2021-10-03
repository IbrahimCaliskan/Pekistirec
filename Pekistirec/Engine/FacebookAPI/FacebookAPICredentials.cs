using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.FacebookAPI
{
    public static class FacebookAPICredentials
    {
        private static string _AppId = null;
        public static string AppId
        {
            get
            {
                if (_AppId == null)
                {
                    _AppId = System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookAppId"];
                }

                return _AppId;
            }
        }

        private static string _RedirectUrl = null;
        public static string RedirectUrl
        {
            get
            {
                if (_RedirectUrl== null)
                {
                    _RedirectUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookRedirectUrl"];
                }
                return _RedirectUrl;
            }
        }

        private static string _AppSecret = null;
        public static string AppSecret
        {
            get
            {
                if (_AppSecret == null)
                {
                    _AppSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookAppSecret"];
                }
                return _AppSecret;
            }

        }

        private static string _response_type = null;
        public static string response_type
        {
            get
            {
                if (_response_type == null)
                {
                    _response_type =  System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookResponseType"] ;
                }
                return _response_type;
            }
        }

        private static string _scope = null;
        public static string scope
        {
            get
            {
                if (_scope == null)
                {
                    _scope = System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookScope"];
                }
                return _scope;
            }
        }        
    }
}