using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.AdminModels
{
    public class KullaniciAyrintiViewModel
    {
        public DataLayer.DomainClasses.BaseDomainClasses.AspNetUser User { get; set; }
        public List<string> Roller { get; set; }
    }
}