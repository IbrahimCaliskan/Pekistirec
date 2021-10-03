using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.DokumanDomainClasses
{
    [Table("DokumanEtiketleri")]
    public class DokumanEtiket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Dokuman")]
        public System.Guid DokumanId { get; set; }
        public virtual Dokuman Dokuman { get; set; }

        [ForeignKey("Etiket")]
        public int EtiketId { get; set; }
        public DataLayer.DomainClasses.BaseDomainClasses.Etiket Etiket { get; set; }

    }
}
