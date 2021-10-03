using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.DokumanModels
{
    public class DokumanIndexViewModel
    {
        public List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman> dokumanlar { get; set; }
    }
}