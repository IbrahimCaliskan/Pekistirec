using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IRepository<E, UoW> : IDisposable
    {        
        IQueryable<E> All { get; }
        IQueryable<E> AllIncluding(params Expression<Func<E, object>>[] includeProperties);
        E Find(params object[] keyValues);
        void InsertOrUpdate(E entitiy, bool ForceInsert = false);
        void Delete(params object[] keyValues);
        int Save();
    }
}
