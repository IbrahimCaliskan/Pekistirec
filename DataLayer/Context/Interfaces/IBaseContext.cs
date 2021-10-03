using DataLayer.DomainClasses.BaseDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context.Interfaces
{
    public interface IBaseContext : IContext
    {
        DbSet<AspNetRole> AspNetRoles { get; set; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<AspNetFriendUser> AspNetFriendUsers { get; set; }
        DbSet<AspNetUserTakipEdilenEtiket> AspNetUserTakipEdilenEtiketler { get; set; }


        DbSet<Etiket> Etiketler { get; set; }
        DbSet<Ayar> Ayarlar { get; set; }
        DbSet<GoogleRefreshToken> GoogleRefreshTokenlari { get; set; }
    }
}
