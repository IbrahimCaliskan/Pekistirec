using DataLayer.DomainClasses.FtsDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context.Interfaces
{
    public interface IFtsContext : IContext
    {
        DbSet<FtsDokuman> FtsDokumanlar { get; set; }
    }
}
