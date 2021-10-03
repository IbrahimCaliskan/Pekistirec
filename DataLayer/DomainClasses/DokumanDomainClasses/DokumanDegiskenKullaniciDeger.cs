using DataLayer.DomainClasses.BaseDomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.DokumanDomainClasses
{
    [Table("DokumanDegiskenKullaniciDegerleri")]
    public class DokumanDegiskenKullaniciDeger
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }

        [ForeignKey("DokumanDegisken")]
        public int DokumanDegiskenId { get; set; }
        public DokumanDegisken DokumanDegisken { get; set; }

        public string Degisken { get; set; }
        
        public string Deger { get; set; }        
    }

    //[Table("DokumanDegiskenKullaniciDegerleri")]
    //public class DokumanDegiskenKullaniciDeger
    //{
    //    [Key]
    //    public int Id { get; set; }
        
    //    [ForeignKey("DokumanDegisken")]
    //    public int DokumanDegiskenId { get; set; }
    //    public DokumanDegisken DokumanDegisken { get; set; }

    //    [ForeignKey("AspNetUser")]
    //    public string AspNetUserId{ get; set; }
    //    public AspNetUser AspNetUser { get; set; }

    //    public string Deger { get; set; }
    //}
}
