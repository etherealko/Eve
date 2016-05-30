using eth.Eve.Storage.Model;
using System.Data.Entity;

namespace eth.Eve.Storage
{
    public class EveDb : DbContext
    {
        static EveDb()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EveDb>());
        }

        public virtual DbSet<PluginStoreString> PluginStoreStrings { get; set; }
        public virtual DbSet<PluginStoreBinary> PluginStoreBinaries { get; set; }
        
        public EveDb(string nameOrConnectionString = "name=EveDb") : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PluginStoreString>().HasKey(e => new { e.PluginGuid, e.Key });
            modelBuilder.Entity<PluginStoreBinary>().HasKey(e => new { e.PluginGuid, e.Key });
            
            modelBuilder.Entity<PluginStoreBinary>().HasRequired(e => e.Value);
        }
    }
}