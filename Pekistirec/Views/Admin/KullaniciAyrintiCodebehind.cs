using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Admin
{
    public class KullaniciAyrintiCodebehind
    {
        public List<string> Roller = Enum.GetNames(typeof(Pekistirec.Engine.AspNetIdentity.Roller)).ToList();
    }
}