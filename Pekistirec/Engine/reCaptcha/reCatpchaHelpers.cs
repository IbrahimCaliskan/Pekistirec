using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.reCaptcha
{
    //Aşağıdaki kodları Web Sayfasına Ekle 

    //{ string reCaptchaPublicKey =  System.Web.Configuration.WebConfigurationManager.AppSettings["reCaptchaPublicKey"]; }
    //<script type="text/javascript" src="http://www.google.com/recaptcha/api/challenge?k=@reCaptchaPublicKey"></script>
    //<noscript>
    //    <iframe src="http://www.google.com/recaptcha/api/noscript?k=@reCaptchaPublicKey"
    //            height="300" width="500" frameborder="0"></iframe><br>
    //    <textarea name="recaptcha_challenge_field" rows="3" cols="40"></textarea>
    //    <input type="hidden" name="recaptcha_response_field"
    //            value="manual_challenge">
    //</noscript>

    public class reCaptchaHelpers
    {     
        public static reCaptchaResult Verify(string user_ip, string recaptcha_challenge_field, string recaptcha_response_field)
        {
            if (reCaptchaSettings.RECAPTCHA_ENABLED)
            {
                string url = "http://www.google.com/recaptcha/api/verify";

                Dictionary<string, string> Data = new Dictionary<string, string>();
                Data.Add("privatekey", reCaptchaCredentials.reCaptchaPrivateKey);
                Data.Add("remoteip", user_ip);
                Data.Add("challenge", recaptcha_challenge_field);
                Data.Add("response", recaptcha_response_field);

                var request = new reCaptchaMyWebRequest(url);
                string response = new reCaptchaMyWebRequest(url, "POST", Data).GetResponse();

                int SatirSonu = response.IndexOf("\n");

                var a = new reCaptchaResult()
                {
                    Success = Convert.ToBoolean(response.Substring(0, SatirSonu)),
                    Message = response.Substring(SatirSonu + 1, response.Length - (SatirSonu + 1))
                };
                return a;
            }
            return new reCaptchaResult() { Success = true, Message = "" };
        }
    }

    public class reCaptchaResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}