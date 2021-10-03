using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DataLayer.DomainClasses.BaseDomainClasses
{
    public partial class AspNetUserClaim
    {
        public int Id { get; set; }

        [Required]
        [StringLength(PROPERTY_ATTRIBUTE_VALUES.ASPNETUSERCLAIM_UserId_MAXLENGTH)]
        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
