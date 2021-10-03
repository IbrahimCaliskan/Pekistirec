using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    [Table("Etiketler")]
    public class Etiket
    {
        [Key]       
        public int Id { get; set; }

        [MaxLength(PROPERTY_ATTRIBUTE_VALUES.ETIKET_Ad_MAXLENGTH)]
        public string Ad { get; set; }

        public EtiketTurleri Tur { get; set; }        
    }
}
