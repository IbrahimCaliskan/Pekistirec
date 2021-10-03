using DataLayer.Context.Interfaces;
using DataLayer.DomainClasses.BaseDomainClasses;
using DataLayer.DomainClasses.DokumanDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class FullContext : DbContext, IBaseContext, IDokumanContext
    {
        public FullContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FullContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            var conv = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
                    "SoftDeleteColumnName",
                    (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(conv);

            #region BaseContextOnModelCreating

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<DomainClasses.BaseDomainClasses.AspNetUser>()
                        .HasMany(u => u.AspNetUserTakipEdilenEtiketler)
                        .WithOptional()
                        .HasForeignKey(u => u.AspNetUserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<DomainClasses.DokumanDomainClasses.Dokuman>()
                        .HasMany(u => u.Etiketler)
                        .WithOptional()
                        .HasForeignKey(u => u.DokumanId)
                        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<DomainClasses.AspNetUser>()
            //            .HasMany(u => u.ArkadasiOlduklarim)
            //            .WithRequired()
            //            .HasForeignKey(u => u.FriendUserId)
            //            .WillCascadeOnDelete(false); ;

            #endregion

            #region DokumanContextOnModelCreating

            modelBuilder.Entity<Dokuman>()
                        .HasMany(u => u.DokumanDegiskenleri)
                        .WithRequired()
                        .HasForeignKey(u => u.DokumanId);
            //.WillCascadeOnDelete(false); 

            #endregion

        }

        #region IBaseContext

        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetFriendUser> AspNetFriendUsers { get; set; }
        public DbSet<AspNetUserTakipEdilenEtiket> AspNetUserTakipEdilenEtiketler { get; set; }

        public DbSet<Etiket> Etiketler { get; set; }
        public DbSet<Ayar> Ayarlar { get; set; }
        public DbSet<GoogleRefreshToken> GoogleRefreshTokenlari { get; set; }

        #endregion

        #region IDokumanContext
        public DbSet<Dokuman> Dokumanlar { get; set; }
        public DbSet<DokumanDegisken> DokumanDegiskenleri { get; set; }
        public DbSet<DokumanDegiskenKullaniciDeger> DokumanDegiskenKullaniciDegerleri { get; set; }
        public DbSet<DokumanLink> DokumanLinkleri { get; set; }

        public DbSet<DokumanEtiket> DokumanEtiketleri { get; set; }
        #endregion
    }
}
