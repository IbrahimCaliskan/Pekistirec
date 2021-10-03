using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Dokuman
{
    public class IndexCodebehind
    {
        public string jsOzelliklerArrayBuffer = "";
        public IndexCodebehind()
        {
            jsOzelliklerArrayBuffer = Pekistirec.Engine.PekistirecBuffers.jsBuffers.OzelliklerArrayBuffer;
        }
    }
}