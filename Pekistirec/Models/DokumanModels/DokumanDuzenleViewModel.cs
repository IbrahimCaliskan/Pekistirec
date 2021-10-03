using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.DokumanModels
{
    public class DokumanDuzenleDegiskenViewModel
    {
        public bool Sil { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }        
    }

    public class DokumanDuzenleViewModel
    {
        public bool Sil { get; set; }
        public DataLayer.DomainClasses.DokumanDomainClasses.Dokuman Dokuman { get; set; }
        public List<DokumanDuzenleDegiskenViewModel> Degiskenler { get; set; }        
    }
}