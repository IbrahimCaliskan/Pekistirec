using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Pekistirec.Engine.FacebookAPI
{
    public class FacebookAPIHelpers
    {
        private static string _LoginURL = null;
        public static string LoginURL
        {
            get
            {
                if (_LoginURL == null)
                {
                    StringBuilder url = new StringBuilder();
                    url.Append("https://www.facebook.com/dialog/oauth?");
                    url.Append(String.Format("client_id={0}", FacebookAPICredentials.AppId));
                    url.Append(String.Format("&redirect_uri={0}", FacebookAPICredentials.RedirectUrl));
                    url.Append(String.Format("&response_type={0}", FacebookAPICredentials.response_type));
                    url.Append(String.Format("&scope={0}", FacebookAPICredentials.scope));
                    _LoginURL = url.ToString();
                }

                return _LoginURL;
            }
        }

        public static FacebookLoginResult Login(string code)
        {
            FacebookLoginResult returnValue = new FacebookLoginResult() { Success = false, Message = "", AccessToken = "", User = new FacebookUser() };

            string accessToken = "";
            //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ Access Token Alınıyor  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            #region Access Token Alınıyor
            try
            {
                string url = String.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}"
                                , FacebookAPICredentials.AppId
                                , HttpUtility.UrlEncode(FacebookAPICredentials.RedirectUrl)
                                , FacebookAPICredentials.AppSecret
                                , code);

                var request = new FacebookAPIMyWebRequest(url, "GET");
                var response = request.GetResponse();

                var uri = new Uri("http://blah.co?" + response);
                var nv = HttpUtility.ParseQueryString(uri.Query);
                accessToken = nv["access_token"];
            }
            catch (Exception)
            {
                returnValue.Message = "AccessToken alınırken hata oluştu.";
                return returnValue;
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                returnValue.Message = "AccessToken boş döndü.";
                return returnValue;
            }

            #endregion
            //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            FacebookUser User = new FacebookUser();
            //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ Kullanıcı Bilgileri Alınıyor  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            #region Kullanıcı Bilgileri Alınıyor

            try
            {
                var UserRequest = new FacebookAPIMyWebRequest("https://graph.facebook.com/me?access_token=" + accessToken, "GET");
                User = JsonConvert.DeserializeObject<FacebookUser>(UserRequest.GetResponse());
            }
            catch (Exception)
            {
                returnValue.Message = "Kullanıcı bilgileri alınırken hata oluştu.";
                return returnValue;
            }

            if (string.IsNullOrEmpty(User.id))
            {
                returnValue.Message = "Kullanıcı bilgileri alınamadı.";
                return returnValue;
            }

            #endregion
            //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            returnValue.Success = true;
            returnValue.AccessToken = accessToken;
            returnValue.User = User;
            returnValue.Message = "Kullanıcı bilgileri alındı.";

            return returnValue;
        }
    }
}