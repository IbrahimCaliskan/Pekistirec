using DataLayer.DomainClasses.FtsDomainClasses;
using DataLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.FtsRepositories
{
    public class FtsDokumanRepository : GenericRepository<IFtsUnitOfWork, FtsDokuman, FtsUnitOfWork>
    {
        public FtsDokumanRepository(IFtsUnitOfWork uow) : base(uow) { }
        public FtsDokumanRepository() : base() { }
    }
}
