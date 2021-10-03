using DataLayer.DomainClasses.BaseDomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.DomainClasses.DokumanDomainClasses
{    
    [Table("Dokumanlar")]       
    [SoftDelete("IsDeleted")]
    public class Dokuman
    {
        [Key]
        public System.Guid Id { get; set; }    

        [ForeignKey("EkleyenAspNetUser")]
        public string EkleyenAspNetUserId { get; set; }
        public AspNetUser EkleyenAspNetUser { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        [ForeignKey("OnaylayanAspNetUser")]        
        public string OnaylayanAspNetUserId { get; set; }
        public AspNetUser OnaylayanAspNetUser { get; set; }

        public DateTime? OnaylanmaTarihi { get; set; }

        public DokumanOnayDurumu OnayDurumu { get; set; }

        [DisplayName("Başlık")]
        [Required]
        [MinLength(PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MINLENGTH)]
        [MaxLength(PROPERTY_ATTRIBUTE_VALUES.DOKUMAN_Baslik_MAXLENGTH)]
        public string Baslik { get; set; }        

        [Required]
        public string Uzanti { get; set; }

        [Required]
        public bool Ozel { get; set; }

        public string EditKey { get; set; }

        public bool IsDeleted { get; set; }
        public List<DokumanEtiket> Etiketler { get; set; }
        public List<DokumanDegisken> DokumanDegiskenleri { get; set; }
        }
}