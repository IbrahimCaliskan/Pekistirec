using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.DokumanModels
{
    public class DokumanDViewModel
    {
        public List<DataLayer.DomainClasses.DokumanDomainClasses.DokumanLink> OncedenKaydedilmisDokumanlar { get; set; }

        public DataLayer.DomainClasses.DokumanDomainClasses.Dokuman Dokuman { get; set; }

        public List<DokumanDegiskenlerinDegerleri> DegiskenDegerleri { get; set; }
    }

    public class DokumanDegiskenlerinDegerleri
    {
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public string Deger { get; set; }
    }

}