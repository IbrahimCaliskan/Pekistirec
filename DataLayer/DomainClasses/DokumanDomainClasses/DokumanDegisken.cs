using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.DokumanDomainClasses
{
    [Table("DokumanDegiskenleri")]
    public class DokumanDegisken
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Dokuman")]
        public System.Guid DokumanId { get; set; }
        public Dokuman Dokuman { get; set; }

        [MaxLength(PROPERTY_ATTRIBUTE_VALUES.DOKUMANDEGISKEN_Ad_MAXLENGTH)]
        public string Ad { get; set; }

        public string Aciklama { get; set; }        
    }
}
