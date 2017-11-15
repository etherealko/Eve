using eth.Eve.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;

namespace eth.MacTestApp
{
    public class EveDbFactory : IDesignTimeDbContextFactory<EveDb>
    {
        public EveDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EveDb>();
            
            builder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=EveDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");

            return new EveDb(builder.Options);
        }
    }
}




