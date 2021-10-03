using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.DokumanModels
{
    public class DokumanYuklediklerimViewModel
    {
        public List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman> Dokumanlar { get; set; }
        public string SiteRootURL { get; set; }
    }
}