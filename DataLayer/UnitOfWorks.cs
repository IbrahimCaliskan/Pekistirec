using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWorks
{
    public interface IBaseUnitOfWork : IUnitOfWork { }
    public class BaseUnitOfWork : GenericUnitOfWork, IBaseUnitOfWork
    {
        public BaseUnitOfWork(DataLayer.Context.Interfaces.IBaseContext context) : base(context) { }
        public BaseUnitOfWork() : base(new BaseContext()) { }
    }

    public interface IDokumanUnitOfWork : IBaseUnitOfWork { }

    public class DokumanUnitOfWork : BaseUnitOfWork, IDokumanUnitOfWork
    {
        public DokumanUnitOfWork(DataLayer.Context.Interfaces.IDokumanContext context) : base(context) { }
        public DokumanUnitOfWork() : base(new DokumanContext()) { }
    }

    public interface IFtsUnitOfWork : IBaseUnitOfWork { }

    public class FtsUnitOfWork : GenericUnitOfWork, IFtsUnitOfWork
    {
        public FtsUnitOfWork(DataLayer.Context.Interfaces.IFtsContext context) : base(context) { }
        public FtsUnitOfWork() : base(new FtsContext()) { }
    }
}
