using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    [Table("AspNetUserTakipEdilenEtiketler")]
    public class AspNetUserTakipEdilenEtiket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }

        [ForeignKey("Etiket")]
        public int EtiketId { get; set; }
        public Etiket  Etiket { get; set; }
    }
}
