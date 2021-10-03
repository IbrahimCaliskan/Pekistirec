using DataLayer.Context.Interfaces;
using DataLayer.DomainClasses.BaseDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class BaseContext : DbContext, IBaseContext
    {

        public BaseContext()
            : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            var conv = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
                    "SoftDeleteColumnName",
                    (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(conv);
        }

        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetFriendUser> AspNetFriendUsers { get; set; }
        public DbSet<AspNetUserTakipEdilenEtiket> AspNetUserTakipEdilenEtiketler { get; set; }

        public DbSet<Etiket> Etiketler { get; set; }
        public DbSet<Ayar> Ayarlar { get; set; }
        public DbSet<GoogleRefreshToken> GoogleRefreshTokenlari { get; set; }
        
    }
}
