using DataLayer.Context.Interfaces;
using DataLayer.DomainClasses.FtsDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{

    public class FtsContext : DbContext, IFtsContext
    {
        public FtsContext()
            : base("FtsConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<FtsContext>());            
        }

        public DbSet<FtsDokuman> FtsDokumanlar { get; set; }
    }
}
