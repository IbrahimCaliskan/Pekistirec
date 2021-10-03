using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.DokumanDomainClasses
{
    [Table("DokumanLinkleri")]
    public class DokumanLink
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Dokuman")]
        public System.Guid DokumanId { get; set; }
        public Dokuman Dokuman { get; set; }

        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }
        public DataLayer.DomainClasses.BaseDomainClasses.AspNetUser AspNetUser { get; set; }

        public string TakmaIsim { get; set; }

        public string Link { get; set; }

        public DateTime EklenmeTarihi { get; set; }
    }
}
