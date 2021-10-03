using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetRoles = new HashSet<AspNetRole>();            
        }

        [Key]
        public string Id { get; set; }

        [StringLength(PROPERTY_ATTRIBUTE_VALUES.ASPNETUSER_Email_MAXLENGTH)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
        
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public string Avatar { get; set; }

        public string Konum { get; set; }

        [MaxLength(PROPERTY_ATTRIBUTE_VALUES.ASPNETUSER_Hakkimda_MAXLENGTH)]
        public string Hakkimda { get; set; }        

        public string GoogleUserId { get; set; }

        public string GoogleRefreshToken { get; set; }
        public string GoogleEmail { get; set; }

        public string FacebookUserId { get; set; }

        public ICollection<AspNetUserTakipEdilenEtiket> AspNetUserTakipEdilenEtiketler { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}
