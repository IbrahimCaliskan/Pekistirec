using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    public class AspNetFriendUser 
    {
        [Key, ForeignKey("User"), Column(Order=1)]
        [Required]
        public string AspNetUserId { get; set; }
        public AspNetUser User { get; set; }

        [Required]
        [Key, ForeignKey("Friend"), Column(Order=2)]
        public string FriendAspNetUserId { get; set; }
        public AspNetUser Friend { get; set; }

        public bool Accepted { get; set; }
    }
}
