using DataLayer.DomainClasses.DokumanDomainClasses;
using DataLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.DokumanRepositories
{
    public class DokumanRepository : GenericRepository<IDokumanUnitOfWork, Dokuman, DokumanUnitOfWork>
    {
        public DokumanRepository(IDokumanUnitOfWork uow) : base(uow) { }
        public DokumanRepository() : base() { }       
    }

    public class DokumanEtiketRepository : GenericRepository<IDokumanUnitOfWork, DokumanEtiket, DokumanUnitOfWork>
    {
        public DokumanEtiketRepository(IDokumanUnitOfWork uow) : base(uow) { }
        public DokumanEtiketRepository() : base() { }
    }

    public class DokumanDegiskenRepository : GenericRepository<IDokumanUnitOfWork, DokumanDegisken, DokumanUnitOfWork>
    {
        public DokumanDegiskenRepository(IDokumanUnitOfWork uow) : base(uow) { }
        public DokumanDegiskenRepository() : base() { }
    }

    public class DokumanDegiskenKullaniciDegerRepository : GenericRepository<IDokumanUnitOfWork,DokumanDegiskenKullaniciDeger, DokumanUnitOfWork>
    {
        public DokumanDegiskenKullaniciDegerRepository(IDokumanUnitOfWork uow) : base(uow) { }
        public DokumanDegiskenKullaniciDegerRepository() : base() { }
    }

    public class DokumanLinkRepository : GenericRepository<IDokumanUnitOfWork, DokumanLink, DokumanUnitOfWork>
    {
        public DokumanLinkRepository(IDokumanUnitOfWork uow) : base(uow) { }
        public DokumanLinkRepository() : base() { }
    }
}
