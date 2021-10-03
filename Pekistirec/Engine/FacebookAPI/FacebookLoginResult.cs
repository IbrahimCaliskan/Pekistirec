using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.FacebookAPI
{
    public class FacebookLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public FacebookUser User { get; set; }
    }
}