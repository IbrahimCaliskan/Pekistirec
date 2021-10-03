using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.FtsDomainClasses
{
    [Table("FtsDokumanlar")]
    public class FtsDokuman
    {
        [Key]
        public int Id { get; set; }
        public Guid DokumanId { get; set; }

        [MaxLength(DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MAXLENGTH)]
        [MinLength(DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MINLENGTH)]
        [Required]
        public string Baslik { get; set; }
        
        [MaxLength(4)]
        public string doctype { get; set; }
        public string docexcerpt { get; set; }

        [Column(TypeName="VARBINARY(MAX)")]
        public byte[] doccontent { get; set; }

        [MaxLength(500)]
        public string Etiket { get; set; }
    }

    //[Table("FtsDokumanlar")]
    //public class FtsDokuman
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public Guid DokumanId { get; set; }

    //    [MaxLength(DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MAXLENGTH)]
    //    [MinLength(DataLayer.DomainClasses.DokumanDomainClasses.PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MINLENGTH)]
    //    [Required]
    //    public string Baslik { get; set; }
        
    //    public string Icerik { get; set; }
    //}
}
