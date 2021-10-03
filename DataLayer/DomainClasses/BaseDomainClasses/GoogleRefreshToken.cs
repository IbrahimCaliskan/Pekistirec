using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    
    [Table("GoogleRefreshTokenlari")]
    public class GoogleRefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string GoogleId { get; set; }
        public string Mail { get; set; }
        public string RefreshToken { get; set; }
    }
}
