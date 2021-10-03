using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.ParameterTampering
{
    public class TamperedParameter
    {
        public string AntiForgeryToken { get; set; }
        public object Parameter { get; set; }
        public string Ip { get; set; }
        public DateTime Time { get; set; }

        public TamperedParameter(string antiForgeryToken, object parameter, string ip)
        {
            this.AntiForgeryToken = antiForgeryToken;
            this.Time = DateTime.Now;
            this.Parameter = parameter;
            this.Ip = ip;
        }
    }    
}