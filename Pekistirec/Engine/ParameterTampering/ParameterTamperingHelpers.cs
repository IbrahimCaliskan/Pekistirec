using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.ParameterTampering
{
    public class ParameterTamperingHelpers
    {
        private static List<TamperedParameter> ParameterList = new List<TamperedParameter>();

        private static string MvcStringToString(System.Web.Mvc.MvcHtmlString antiForgeryToken)
        {
            string token = antiForgeryToken.ToString();
            token = token.Substring(token.IndexOf("value=\"") + 7);
            token = token.Substring(0, token.IndexOf("\""));
            return token;
        }


        public static void Ekle(System.Web.Mvc.MvcHtmlString antiForgeryToken, object parameter, string ip)
        {
            lock (ParameterList)
            {
                ParameterList.Add(new TamperedParameter(MvcStringToString(antiForgeryToken), parameter, ip));
            }
        }

        public static TamperedParameter Getir(string antiForgeryToken, string ip)
        {
            TamperedParameter parameter = null;

            lock (ParameterList)
            {
                // Temizlik yapılıyor...
                var silinecekListe = ParameterList.Where(p => p.Time < DateTime.Now.AddMinutes(-10));
                foreach (var item in silinecekListe)
                {
                    ParameterList.Remove(item);
                }

                parameter = ParameterList.Where(p => p.Ip == ip && p.AntiForgeryToken == antiForgeryToken).LastOrDefault();
            }

            return (parameter == null) ? null : parameter;
        }
    }
}