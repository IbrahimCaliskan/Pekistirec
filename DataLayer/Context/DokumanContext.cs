using DataLayer.Context.Interfaces;
using DataLayer.DomainClasses.DokumanDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class DokumanContext : BaseContext, IDokumanContext
    {
        public DbSet<Dokuman> Dokumanlar { get; set; }        
        public DbSet<DokumanDegisken> DokumanDegiskenleri { get; set; }
        public DbSet<DokumanDegiskenKullaniciDeger> DokumanDegiskenKullaniciDegerleri { get; set; }
        public DbSet<DokumanLink> DokumanLinkleri { get; set; }
        public DbSet<DokumanEtiket> DokumanEtiketleri { get; set; }
    }
}
