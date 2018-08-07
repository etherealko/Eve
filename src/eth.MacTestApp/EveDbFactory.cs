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
            
            // this connection string is used during the db deploy process only (commandline dotnet ef database update)

            builder.UseMySql(@"server=localhost;port=3306;database=eveDb;uid=root;password=password");
            //builder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=EveDb;integrated security=True;MultipleActiveResultSets=True");

            return new EveDb(builder.Options);
        }
    }
}




