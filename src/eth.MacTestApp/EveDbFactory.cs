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
            
            builder.UseMySql(@"server=localhost;port=3306;database=eveDb;uid=root;password=password");

            return new EveDb(builder.Options);
        }
    }
}




