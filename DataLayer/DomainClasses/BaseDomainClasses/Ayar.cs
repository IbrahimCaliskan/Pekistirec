using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    [Table("Ayarlar")]
    public class Ayar
    {
        [Key]
        public string AyarAdi { get; set; }
        public string Deger { get; set; }

    }
}
