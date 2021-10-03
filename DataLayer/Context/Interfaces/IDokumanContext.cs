
using DataLayer.DomainClasses.DokumanDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context.Interfaces
{
    public interface IDokumanContext : IBaseContext
    {
        DbSet<Dokuman> Dokumanlar { get; set; }        
        DbSet<DokumanDegisken> DokumanDegiskenleri { get; set; }
        DbSet<DokumanDegiskenKullaniciDeger> DokumanDegiskenKullaniciDegerleri { get; set; }
        DbSet<DokumanLink> DokumanLinkleri { get; set; }
        DbSet<DokumanEtiket> DokumanEtiketleri { get; set; }
    }
}
