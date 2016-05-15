using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ExterniUredjajServis.Uposlenici.Models
{
    public class UposlenikDbContext:DbContext
    {
        public DbSet<Uposlenik> Uposlenici { get; set; }
        public DbSet<Dogadjaj> Dogadjaji { get; set; }
        //za onemogucavanje automatskog dodavanja mnozine
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}