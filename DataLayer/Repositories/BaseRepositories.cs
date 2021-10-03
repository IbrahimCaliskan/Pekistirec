using DataLayer.DomainClasses.BaseDomainClasses;
using DataLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.BaseRepositories
{
    public class EtiketRepository : GenericRepository<IBaseUnitOfWork, Etiket, BaseUnitOfWork>
    {
        //public static Action Saved;

        public EtiketRepository(IBaseUnitOfWork uow) : base(uow) { }
        public EtiketRepository() : base() { }
    }

    public class AspNetUserOzellikRepository : GenericRepository<IBaseUnitOfWork, AspNetUserTakipEdilenEtiket, BaseUnitOfWork>
    {
        public AspNetUserOzellikRepository(IBaseUnitOfWork uow) : base(uow) { }
        public AspNetUserOzellikRepository() : base() { }
    }

    public class AspNetUserRepository : GenericRepository<IBaseUnitOfWork, AspNetUser, BaseUnitOfWork>
    {
        public AspNetUserRepository(IBaseUnitOfWork uow) : base(uow) { }
        public AspNetUserRepository() : base() { }
    }

    public class AspNetFriendUserRepository : GenericRepository<IBaseUnitOfWork, AspNetFriendUser, BaseUnitOfWork>
    {
        public AspNetFriendUserRepository(IBaseUnitOfWork uow) : base(uow) { }
        public AspNetFriendUserRepository() : base() { }
    }

    public class AyarlarRepository : GenericRepository<IBaseUnitOfWork, Ayar, BaseUnitOfWork>
    {
        public AyarlarRepository(IBaseUnitOfWork uow) : base(uow) { }
        public AyarlarRepository() : base() { }
    }

    public class GoogleRefreshTokenRepository : GenericRepository<IBaseUnitOfWork, GoogleRefreshToken, BaseUnitOfWork>
    {
        public GoogleRefreshTokenRepository(IBaseUnitOfWork uow) : base(uow) { }
        public GoogleRefreshTokenRepository() : base() { }
    }

}
