using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.PekistirecTypes
{
    public class JsonReturnResult
    {
        public string status { get; set; }
        public string message { get; set; }

        public JsonReturnResult(string status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}