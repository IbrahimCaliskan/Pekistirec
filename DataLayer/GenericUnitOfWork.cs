using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public abstract class GenericUnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly IContext _context;

        internal IContext Context
        {
            get { return _context; }
        }

        public GenericUnitOfWork(IContext context)
        {
            _context = context;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }   
}
